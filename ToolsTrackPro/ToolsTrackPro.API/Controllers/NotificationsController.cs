using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsTrackPro.API.Models;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Tools.Commands;

namespace ToolsTrackPro.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all transactions by tool id
        /// </summary>
        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<NotificationDto>>> GetNotificationsByUserId(int id)
        {
            var notifications = await _mediator.Send(new GetNotificationsByUserIdQuery() { UserId = id});
            return Ok(new ApiResponse<List<NotificationDto>>("success", notifications));
        }

        /// <summary>
        /// Borrow tool
        /// </summary>
        [HttpPut("{id}/read")]
        public async Task<ActionResult<int>> MarkRead(int id)
        {
            var added = await _mediator.Send(new MarkNotificationAsReadCommand() { Id = id});
            return Ok(new ApiResponse<ToolDto>(added ? "success": "fail"));
        }
    }
}
