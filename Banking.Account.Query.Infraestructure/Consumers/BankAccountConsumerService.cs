using Banking.Account.Query.Application.Contracts.Persitence;
using Banking.Account.Query.Application.Models;
using Banking.Account.Query.Domain;
using Banking.Cqrs.Core.Events;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Banking.Account.Query.Infraestructure.Consumers
{
    public class BankAccountConsumerService : IHostedService
    {

        private readonly IBankAccountRepository _bankAccountRepository;
        public KafkaSettings _kafkaSettings { get; }

        public BankAccountConsumerService(IServiceScopeFactory factory)
        {
            _bankAccountRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IBankAccountRepository>();
            _kafkaSettings = factory.CreateScope().ServiceProvider.GetRequiredService<IOptions<KafkaSettings>>().Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _kafkaSettings.GroupId,
                BootstrapServers = $"{_kafkaSettings.HostName}:{_kafkaSettings.Port}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {

                    var bankTopics = new string[]
                    {
                        typeof(AccountClosedEvent).Name,
                        typeof(AccountOpenedEvent).Name,
                        typeof(FundsDepositedEvent).Name,
                        typeof(FundsWithdrawEvent).Name,
                    };

                    var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = $"{_kafkaSettings.HostName}:{_kafkaSettings.Port}" }).Build();

                    foreach (var topic in bankTopics)
                    {
                        var topicExists = adminClient.GetMetadata(TimeSpan.FromSeconds(5)).Topics.Exists(t => t.Topic.Equals(topic));

                        if (!topicExists)
                        {
                            // Crear el topic manualmente con las configuraciones deseadas
                            var topicConfig = new TopicSpecification
                            {
                                Name = topic,
                                NumPartitions = 1, // Número de particiones deseado
                                ReplicationFactor = 1 // Factor de replicación deseado
                            };

                            adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicConfig }).Wait();
                        }
                    }


                    consumerBuilder.Subscribe(bankTopics);
                    var cancelToken = new CancellationTokenSource();

                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuilder.Consume(cancelToken.Token);

                            if (consumer.Topic.Equals(typeof(AccountOpenedEvent).Name))
                            {
                                var accountOpenedEvent = JsonConvert.DeserializeObject<AccountOpenedEvent>(consumer.Message.Value);

                                var bankAccount = new BankAccount
                                {
                                    Identifier = accountOpenedEvent.Id,
                                    AccountHolder = accountOpenedEvent.AccountHolder,
                                    AccountType = accountOpenedEvent.AccountType,
                                    Balance = accountOpenedEvent.OpeningBalance,
                                    CreationDate = accountOpenedEvent.CreatedDate,
                                };
                                _bankAccountRepository.AddAsync(bankAccount).Wait();
                            }


                            if (consumer.Topic.Equals(typeof(AccountClosedEvent).Name))
                            {
                                var accountClosedEvent = JsonConvert.DeserializeObject<AccountClosedEvent>(consumer.Message.Value);
                                _bankAccountRepository.DeleteByIdentifier(accountClosedEvent!.Id).Wait();
                            }

                            if (consumer.Topic.Equals(typeof(FundsDepositedEvent).Name))
                            {
                                var accountDepositEvent = JsonConvert.DeserializeObject<FundsDepositedEvent>(consumer.Message.Value);

                                var bankAccount = new BankAccount
                                {
                                    Identifier = accountDepositEvent!.Id,
                                    Balance = accountDepositEvent.Amount,
                                };
                                _bankAccountRepository.DepositBankAccountByIdentifier(bankAccount).Wait();
                            }

                            if (consumer.Topic.Equals(typeof(FundsWithdrawEvent).Name))
                            {
                                var accountDepositEvent = JsonConvert.DeserializeObject<FundsWithdrawEvent>(consumer.Message.Value);

                                var bankAccount = new BankAccount
                                {
                                    Identifier = accountDepositEvent!.Id,
                                    Balance = accountDepositEvent.Amount,
                                };
                                _bankAccountRepository.WithdrawBankAccountByIdentifier(bankAccount).Wait();
                            }
                        }
                    }catch(OperationCanceledException)
                    {
                        Console.WriteLine("Murio la conexion del consumidor");
                        consumerBuilder.Close();
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Termino kafka");

            return Task.CompletedTask;
        }
    }
}
