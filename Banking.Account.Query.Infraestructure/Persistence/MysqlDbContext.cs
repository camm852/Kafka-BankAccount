using Banking.Account.Query.Domain;
using Microsoft.EntityFrameworkCore;

namespace Banking.Account.Query.Infraestructure.Persistence
{
    public class MysqlDbContext : DbContext
    {
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options) { 
        
        }

        public DbSet<BankAccount>? BankAccounts { get; set; }


    }
}
