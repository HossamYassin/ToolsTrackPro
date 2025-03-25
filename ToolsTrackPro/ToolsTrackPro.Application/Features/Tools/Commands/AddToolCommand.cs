using MediatR;
using ToolsTrackPro.Application.DTOs;

namespace ToolsTrackPro.Application.Features.Tools.Commands
{
    public class AddToolCommand : IRequest<bool>
    {
        public ToolDto Tool { get; }

        public AddToolCommand(ToolDto tool) => Tool = tool;
    }
}
