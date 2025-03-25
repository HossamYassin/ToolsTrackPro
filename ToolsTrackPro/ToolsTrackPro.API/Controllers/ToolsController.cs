using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsTrackPro.API.Models;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Application.Features.Tools.Queries;

namespace ToolsTrackPro.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tools")]
    public class ToolsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ToolsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all tools
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ToolDto>>> GetAll()
        {
            var tools = await _mediator.Send(new GetAllToolsQuery());

            return Ok(new ApiResponse<List<ToolDto>>("success", tools));
        }

        /// <summary>
        /// Get all tools with latest transactions
        /// </summary>
        [HttpGet("transactions")]
        public async Task<ActionResult<List<ToolTransactionDto>>> GetAllWithLatestTransactions()
        {
            var tools = await _mediator.Send(new GetAllToolsWithLatestTransactionsQuery());
            return Ok(new ApiResponse<List<ToolTransactionDto>>("success", tools));
        }

        /// <summary>
        /// Get a tool by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ToolDto>> GetById(int id)
        {
            var tool = await _mediator.Send(new GetToolByIdQuery(id));

            if (tool == null)
                return NotFound(new ApiResponse<ToolDto>("fail", new List<string> { "tool does not exist" }));

            return Ok(new ApiResponse<ToolDto>("success", tool));
        }

        /// <summary>
        /// Add a new tool
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Add([FromBody] ToolDto tool)
        {
            var added = await _mediator.Send(new AddToolCommand(tool));
            return Ok(new ApiResponse<ToolDto>(added ? "success" : "fail"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ToolDto tool)
        {
            if (id != tool.Id) 
                return BadRequest(new ApiResponse<ToolDto>("fail", new List<string> { "ID mismatch." }));

            bool isUpdated = await _mediator.Send(new UpdateToolCommand(tool));
            if (!isUpdated) 
                return NotFound(new ApiResponse<ToolDto>( "fail", new List<string> { "Tool not found." }));

            return Ok(new ApiResponse<ToolDto>("success"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _mediator.Send(new DeleteToolCommand { Id = id });
            if (!isDeleted)
                return NotFound(new ApiResponse<ToolDto>("fail", new List<string> { "Tool not found." }));

            return Ok(new ApiResponse<int>("success"));

        }
    }
}
