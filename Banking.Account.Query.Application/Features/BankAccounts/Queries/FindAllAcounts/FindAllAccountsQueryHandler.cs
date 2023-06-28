using Banking.Account.Query.Application.Contracts.Persitence;
using Banking.Account.Query.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAcounts
{
    public class FindAllAccountsQueryHandler : IRequestHandler<FindAllAccountsQuery, IEnumerable<BankAccount>>
    {

        private readonly IBankAccountRepository _bankAccountRepository;

        public FindAllAccountsQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<IEnumerable<BankAccount>> Handle(FindAllAccountsQuery request, CancellationToken cancellationToken)
        {
            return await _bankAccountRepository.GetAllAsync();
        }
    }
}
