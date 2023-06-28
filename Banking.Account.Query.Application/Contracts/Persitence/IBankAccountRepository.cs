using Banking.Account.Query.Domain;

namespace Banking.Account.Query.Application.Contracts.Persitence
{
    public interface IBankAccountRepository : IAsyncRepository<BankAccount>
    {
        Task<BankAccount> GetByIdentifier(string identifier);
        Task<IEnumerable<BankAccount>> GetByAccountHolder(string accountHolder);
        Task<IEnumerable<BankAccount>> GetByBalanceGreaterThan(double balance);
        Task<IEnumerable<BankAccount>> GetByBalanceLessThan(double balance);
        Task DeleteByIdentifier(string identifier);
        Task DepositBankAccountByIdentifier(BankAccount bankAccount);
        Task WithdrawBankAccountByIdentifier(BankAccount bankAccount);
    }
}
