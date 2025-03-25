using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ToolsTrackPro.API.Hub;
using ToolsTrackPro.API.Models;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Tools.Queries;
using ToolsTrackPro.Application.Features.Transactions.Commands;
using ToolsTrackPro.Application.Features.Transactions.Queries;
using ToolsTrackPro.Domain.Entities;

namespace ToolsTrackPro.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<ToolNotificationHub> _hubContext;

        public TransactionsController(IMediator mediator,
            IHubContext<ToolNotificationHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
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

            if (added)
            {
                var tool = await _mediator.Send(new GetToolByIdQuery(retrun.ToolId));
                await _hubContext.Clients.All.SendAsync("ToolAvailable", tool.Name);
            }

            return Ok(new ApiResponse<ToolDto>(added ? "success" : "fail"));
        }
    }
}
