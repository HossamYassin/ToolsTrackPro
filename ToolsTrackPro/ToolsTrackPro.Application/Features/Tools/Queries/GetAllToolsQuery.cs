using MediatR;
using ToolsTrackPro.Application.DTOs;

namespace ToolsTrackPro.Application.Features.Tools.Queries
{
    public class GetAllToolsQuery : IRequest<List<ToolDto>>
    { 

    }
}
