using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsTrackPro.API.Models;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Transactions.Commands;
using ToolsTrackPro.Application.Features.Transactions.Queries;

namespace ToolsTrackPro.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all transactions by tool id
        /// </summary>
        [HttpGet("tool/{id}")]
        public async Task<ActionResult<List<TransactionViewDto>>> GetAllTransacctionsByToolId(int id)
        {
            var transactions = await _mediator.Send(new GetTransactionsByToolIdQuery() { ToolId = id});
            return Ok(new ApiResponse<List<TransactionViewDto>>("success", transactions));
        }

        /// <summary>
        /// Borrow tool
        /// </summary>
        [HttpPut("borrow")]
        public async Task<ActionResult<int>> Borrow([FromBody] BorrowToolCommand borrow)
        {
            var added = await _mediator.Send(borrow);
            return Ok(new ApiResponse<ToolDto>(added ? "success": "fail"));
        }

        /// <summary>
        /// Return tool
        /// </summary>
        [HttpPut("return")]
        public async Task<IActionResult> Return([FromBody] ReturnToolCommand retrun)
        {
            var added = await _mediator.Send(retrun);
            return Ok(new ApiResponse<ToolDto>(added ? "success" : "fail"));
        }
    }
}
