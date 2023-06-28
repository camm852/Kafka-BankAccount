using Banking.Account.Query.Application.Contracts.Persitence;
using Banking.Account.Query.Domain;
using MediatR;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountByBalance
{
    public class FindByAccountByBalanceQueryHandler : IRequestHandler<FindByAccountByBalanceQuery, IEnumerable<BankAccount>>
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public FindByAccountByBalanceQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<IEnumerable<BankAccount>> Handle(FindByAccountByBalanceQuery request, CancellationToken cancellationToken)
        {
            if(request.EqualityType == "GREATER_THAN")
            {
                return await _bankAccountRepository.GetByBalanceGreaterThan(request.Balance);
            }
            return await _bankAccountRepository.GetByBalanceLessThan(request.Balance);
        }
    }
}
