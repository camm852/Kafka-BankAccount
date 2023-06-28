using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Handlers;
using Banking.Cqrs.Core.Infraestructure;

namespace Banking.Account.Command.Infraestructure.KafkaEvents
{
    public class AccountEventSourcingHandler : IEventSourcingHandler<AccountAggregate>
    {

        private readonly IEventStore _eventStore;

        public AccountEventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<AccountAggregate> GetById(string id)
        {
            var aggregate = new AccountAggregate();
            var events = await _eventStore.GetEvents(id);
            if(events != null && events.Any())
            {
                aggregate.ReplayEvents(events);
                var latestVerion = events.Max(e => e.Version);
                aggregate.SetVersion(latestVerion);
            }

            return aggregate;
        }

        public async Task Save(AggregateRoot aggregate)
        {
            await _eventStore.SaveEvents(aggregate.Id, aggregate.GetUncommitedChanges(), aggregate.GetVersion());
            aggregate.MarkChangesAsCommited();
        }
    }
}
