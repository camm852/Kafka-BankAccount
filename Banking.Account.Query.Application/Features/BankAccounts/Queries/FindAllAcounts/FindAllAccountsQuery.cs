using Banking.Account.Query.Domain;
using MediatR;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAcounts
{
    public class FindAllAccountsQuery : IRequest<IEnumerable<BankAccount>>
    {

    }
}
