using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Application.Features.Tools.Queries;
using ToolsTrackPro.Domain.Entities;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Tools.Handlers
{
    public class GetToolByIdQueryHandler : IRequestHandler<GetToolByIdQuery, ToolDto?>
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;

        public GetToolByIdQueryHandler(IToolRepository toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        public async Task<ToolDto?> Handle(GetToolByIdQuery request, CancellationToken cancellationToken)
        {
            var tool = await _toolRepository.GetByIdAsync(request.Id);
            return tool == null ? null : _mapper.Map<ToolDto>(tool);
        }
    }
}
