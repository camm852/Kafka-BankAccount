using Banking.Account.Command.Application.Aggregates;
using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Domain;
using Banking.Cqrs.Core.Events;
using Banking.Cqrs.Core.Infraestructure;
using Banking.Cqrs.Core.Producers;

namespace Banking.Account.Command.Infraestructure.KafkaEvents
{
    public class AccountEventStore : IEventStore
    {
 
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;

        public AccountEventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }

        public async Task<List<BaseEvent>> GetEvents(string aggregateIdentfier)
        {
            var eventStream = await _eventStoreRepository.FindByAgreggateByIdentifier(aggregateIdentfier);
            if(eventStream == null || !eventStream.Any())
            {
                throw new Exception("La cuenta bancaria no existe");
            }

            return eventStream.Select(x => x.EventData).ToList();

        }

        public async Task SaveEvents(string aggregateIdentfier, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepository.FindByAgreggateByIdentifier(aggregateIdentfier);

            if(expectedVersion != -1 && eventStream.ElementAt(eventStream.Count() - 1).EventData.Version != expectedVersion)
            {
                throw new Exception("Errores de concurrencia");
            }

            var version = expectedVersion;
            
            foreach(var evt in events)
            {
                version++;
                evt.Version = version;
                var eventModel = new EventModel
                {
                    Timestamp = DateTime.Now,
                    AggregateIdentifier = aggregateIdentfier,
                    AggregateType = nameof(AccountAggregate),
                    EventType = evt.GetType().Name,
                    EventData = evt
                };
                await _eventStoreRepository.InsertDocument(eventModel);
                _eventProducer.Produce(evt.GetType().Name, evt);
            }
        }
    }
}
