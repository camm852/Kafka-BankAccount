using Banking.Cqrs.Core.Events;

namespace Banking.Cqrs.Core.Infraestructure
{
    public interface IEventStore
    {
        Task SaveEvents(string aggregateIdentfier, IEnumerable<BaseEvent> events, int expectedVersion);

        Task<List<BaseEvent>> GetEvents(string aggregateIdentfier);

    }
}
