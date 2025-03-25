using MediatR;
using ToolsTrackPro.Application.DTOs;

namespace ToolsTrackPro.Application.Features.Tools.Queries
{
    public class GetToolByIdQuery : IRequest<ToolDto?>
    {
        public int Id { get; }
        public GetToolByIdQuery(int id) => Id = id;
    }
}
