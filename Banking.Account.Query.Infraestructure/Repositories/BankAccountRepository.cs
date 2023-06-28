using Banking.Account.Query.Application.Contracts.Persitence;
using Banking.Account.Query.Domain;
using Banking.Account.Query.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Account.Query.Infraestructure.Repositories
{
    public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(MysqlDbContext context) : base(context)
        {
        }

        public async Task DeleteByIdentifier(string identifier)
        {
            var bankAccount = await _context.BankAccounts!.Where(x => x.Identifier.Equals(identifier)).FirstOrDefaultAsync();
            if(bankAccount == null)
            {
                throw new Exception($"No se puedo eliminar la cuenta con el id {identifier}");
            }
            _context.BankAccounts!.Remove(bankAccount);
            _context.SaveChanges();
        }

        public async Task DepositBankAccountByIdentifier(BankAccount bankAccount)
        {
            var account = await _context.BankAccounts!.Where(x => x.Identifier.Equals(bankAccount.Identifier)).FirstOrDefaultAsync();
            if(account == null)
            {
                throw new Exception($"No se pudo actualizar en mysql la cuenta de banco con identifier {bankAccount.Identifier}");
            }
            account.Balance += bankAccount.Balance;
            await UpdateAsync(account);
        }

        public async Task<IEnumerable<BankAccount>> GetByAccountHolder(string accountHolder)
        {
            return await _context.BankAccounts!.Where(x => x.AccountHolder.Equals(accountHolder)).ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetByBalanceGreaterThan(double balance)
        {
            return await _context.BankAccounts!.Where(x => x.Balance > balance).ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetByBalanceLessThan(double balance)
        {
            return await _context.BankAccounts!.Where(x => x.Balance < balance).ToListAsync();
        }

        public async Task<BankAccount> GetByIdentifier(string identifier)
        {
            return await _context.BankAccounts!.Where(x => x.Identifier.Equals(identifier)).FirstOrDefaultAsync();
        }

        public async Task WithdrawBankAccountByIdentifier(BankAccount bankAccount)
        {
            var account = await _context.BankAccounts!.Where(x => x.Identifier.Equals(bankAccount.Identifier)).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception($"No se pudo actualizar en mysql la cuenta de banco con identifier {bankAccount.Identifier}");
            }

            if(account.Balance < bankAccount.Balance)
            {
                throw new Exception($"El balance de la cuenta es menor que el dinero a retirar para {bankAccount.Identifier}");
            }

            account.Balance -= bankAccount.Balance;
            await UpdateAsync(account);
        }
    }
}
