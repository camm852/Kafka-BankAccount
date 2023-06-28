using Banking.Account.Command.Application.Features.BankAccounts.Commands.CloseAccount;
using Banking.Account.Command.Application.Features.BankAccounts.Commands.DepositeFund;
using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Account.Command.Application.Features.BankAccounts.Commands.WithdrawFunds;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Banking.Account.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private IMediator _mediator;
        public BankAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("open-account", Name="OpenAccount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> OpeAccount([FromBody] OpenAccountCommand command)
        {
            var id = Guid.NewGuid().ToString();
            command.Id = id;
            return await _mediator.Send(command);
        }


        [HttpDelete("delete-account/{id}", Name = "CloseAccount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> CloseAccount(string id)
        {
            var command = new CloseAccountCommand
            {
                Id = id
            };

            return await _mediator.Send(command);
        }

        [HttpPut("deposit-fund/{id}", Name="DepositFund")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> DepositFund(string id, [FromBody] DepositFundsCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpPut("withdraw-fund/{id}", Name = "WithdrawFund")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> WithdrawFund(string id, [FromBody] WithdrawFundsCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }



    }
}
