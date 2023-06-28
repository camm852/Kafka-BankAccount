using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handlers;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.CloseAccount
{
    public class CloseAccountCommandHandle : IRequestHandler<CloseAccountCommand, bool>
    {

        private readonly IEventSourcingHandler<AccountAggregate> _eventSourcingHandle;

        public CloseAccountCommandHandle(IEventSourcingHandler<AccountAggregate> eventSourcingHandle)
        {
            _eventSourcingHandle = eventSourcingHandle;
        }

        public async Task<bool> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandle.GetById(request.Id);
            aggregate.CloseAccount();
            await _eventSourcingHandle.Save(aggregate);
            return true;
        }
    }
}
