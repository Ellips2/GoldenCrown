using AutoMapper;
using FluentValidation;
using GoldenCrown.API.Attributes;
using GoldenCrown.API.Dtos.Finance;
using GoldenCrown.Application.DTOs.Finance;
using GoldenCrown.Application.Features.Deposit;
using GoldenCrown.Application.Features.GetBalance;
using GoldenCrown.Application.Features.GetTransactionHistory;
using GoldenCrown.Application.Features.Transfer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoldenCrown.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MyAuthorize]
    public class FinanceController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FinanceController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalanceAsync(BalanceRequest request, IValidator<BalanceRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var query = new GetBalanceQuery(GetUserId(), request.Currency);
            var balanceResult = await _mediator.Send(query);

            if (balanceResult.IsSuccess)
            {
                return Ok(new BalanceResponse
                {
                    Balance = balanceResult.Value
                });
            }

            return BadRequest(new { Message = balanceResult.ErrorMessage });
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> DepositAsync([FromBody] DepositRequest request, IValidator<DepositRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var command = new DepositCommand(GetUserId(), request.Amount, request.Currency);
            var depositResult = await _mediator.Send(command);
            if (depositResult.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(new {Message = depositResult.ErrorMessage});
        }


        [HttpPost("transfer")]
        public async Task<IActionResult> TransferAsync([FromBody] TransferRequest request, IValidator<TransferRequest> validator, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var command = new TransferCommand(GetUserId(), request.ReceiverLogin, request.Amount, request.Currency);
            var transferResult = await _mediator.Send(command);
            if (transferResult.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(new {Message = transferResult.ErrorMessage});
        }

        [HttpPost("history")]
        public async Task<ActionResult<IEnumerable<TransactionHistoryResponse>>> GetTransactionHistoryAsync([FromQuery] TransactionHistoryRequest request, 
            IValidator<TransactionHistoryRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var query = new GetTransactionHistoryQuery(
                GetUserId(), 
                request.From, 
                request.To,
                request.Offset,
                request.Limit);

            var historyResult = await _mediator.Send(query);

            if (historyResult.IsSuccess)
            {
                var response = historyResult.Value!.Select(_mapper.Map<TransactionHistoryResponse>);
                return Ok(response);
            }

            return BadRequest(new { Message = historyResult.ErrorMessage });
        }

        internal int GetUserId()
        {
            var userId = HttpContext.Items[Constants.UserIdContextParameter] as int?;
            return userId!.Value;
        }
    }
} 
