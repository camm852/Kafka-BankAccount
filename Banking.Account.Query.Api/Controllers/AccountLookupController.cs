using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountByBalance;
using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountByHolder;
using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountById;
using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAcounts;
using Banking.Account.Query.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Banking.Account.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountLookupController : ControllerBase
    {

        private IMediator _mediator;

        public AccountLookupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("accounts", Name="GetAllAccounts")]
        [ProducesResponseType(typeof(IEnumerable<BankAccount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetAllAcounts()
        {
            return Ok(await _mediator.Send(new FindAllAccountsQuery()));
        }

        [HttpGet("account/{id}", Name ="GetAccountByIdentifier")]
        [ProducesResponseType(typeof(IEnumerable<BankAccount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetAccountByIdentifier(string id)
        {
            return Ok(await _mediator.Send(new FindAccountByIdQuery { Identifier = id }));
        }

        [HttpGet("account-balance", Name = "GetAccountByBalance")]
        [ProducesResponseType(typeof(IEnumerable<BankAccount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetAccountByBalance( [FromBody]FindByAccountByBalanceQuery query)
        {
            return Ok(await _mediator.Send(query));
        }


        [HttpGet("account-holder/{name}", Name = "GetAccountByHolder")]
        [ProducesResponseType(typeof(IEnumerable<BankAccount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetAccountByHolder(string name)
        {
            return Ok(await _mediator.Send(new FindAccountByHolderQuery { AccountHolder = name}));
        }


    }
}
