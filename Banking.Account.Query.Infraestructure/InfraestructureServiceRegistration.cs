using Banking.Account.Query.Application.Contracts.Persitence;
using Banking.Account.Query.Infraestructure.Persistence;
using Banking.Account.Query.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Account.Query.Infraestructure
{
    public static class InfraestructureServiceRegistration
    {
        public static IServiceCollection AddInfraestructureServices(this IServiceCollection services, IConfiguration configuration) {


            var connectionString = configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<MysqlDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();


            return services;
        
        }
    }
}
