using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Events;

namespace Banking.Account.Command.Application.Aggregates
{
    public class AccountAggregate : AggregateRoot
    {
        public bool Active { get; set; }
        public double Balance { get; set; }

        public AccountAggregate()
        {
        }

        public AccountAggregate(OpenAccountCommand command)
        {
            var accountOpenedEvent = new AccountOpenedEvent(
                    command.Id,
                    command.AccountHolder,
                    command.AccountType,
                    DateTime.Now,
                    command.OpeningBalance
                );

            RaiseEvent(accountOpenedEvent);
        }

        public void Apply(AccountOpenedEvent @event)
        {
            Id = @event.Id;
            Active = true;
            Balance = @event.OpeningBalance;
        }

        public void DepositFunds(double amount)
        {
            if (!Active)
            {
                throw new Exception("Los fondos no pueden ser depositados en una cuenta inactiva");
            }
            if(amount <= 0)
            {
                throw new Exception("El deposito debe ser mayor que cero");
            }

            var fundsDepositEvent = new FundsDepositedEvent(Id) { 
                Id = Id,
                Amount = amount
            };

            RaiseEvent(fundsDepositEvent);
        }

        public void Apply(FundsDepositedEvent @event)
        {
            Id = @event.Id;
            Balance += @event.Amount;
        }

        public void WithdrawFunds(double amount)
        {
            if (!Active)
            {
                throw new Exception("Los fondos no pueden ser retirados de una cuenta inactiva");
            }
            if (amount <= 0)
            {
                throw new Exception("El retiro debe ser mayor que cero");
            }

            var fundsWithdrawEvent = new FundsWithdrawEvent(Id)
            {
                Id = Id,
                Amount = amount
            };

            RaiseEvent(fundsWithdrawEvent);
        }

        public void Apply(FundsWithdrawEvent @event)
        {
            Id = @event.Id;
            Balance -= @event.Amount;
        }

        public void CloseAccount()
        {
            if (!Active)
            {
                throw new Exception("La cuenta esta cerrada");
            }

            var accountClosedEvent = new AccountClosedEvent(Id);

            RaiseEvent(accountClosedEvent);
        }

        public void Apply(AccountClosedEvent @event)
        {
            Id = @event.Id;
            Active = false;
        }

    }
}
