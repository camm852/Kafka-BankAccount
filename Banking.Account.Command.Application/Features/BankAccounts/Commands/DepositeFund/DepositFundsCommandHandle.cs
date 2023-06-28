using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handlers;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.DepositeFund
{
    public class DepositFundsCommandHandle : IRequestHandler<DepositFundsCommand, bool>
    {
        private readonly IEventSourcingHandler<AccountAggregate> _eventSourcingHandle;

        public DepositFundsCommandHandle(IEventSourcingHandler<AccountAggregate> eventSourcingHandle)
        {
            _eventSourcingHandle = eventSourcingHandle;
        }

        public async Task<bool> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandle.GetById(request.Id);
            aggregate.DepositFunds(request.Amount);
            await _eventSourcingHandle.Save(aggregate);
            return true;
        }
    }
}
