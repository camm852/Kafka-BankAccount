using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.WithdrawFunds
{
    public class WithdrawFundsCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}
