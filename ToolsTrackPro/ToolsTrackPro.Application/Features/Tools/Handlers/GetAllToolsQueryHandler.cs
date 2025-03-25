using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Application.Features.Tools.Queries;
using ToolsTrackPro.Domain.Entities;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Tools.Handlers
{
    public class GetAllToolsQueryHandler : IRequestHandler<GetAllToolsQuery, List<ToolDto>>
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;

        public GetAllToolsQueryHandler(IToolRepository toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        public async Task<List<ToolDto>> Handle(GetAllToolsQuery request, CancellationToken cancellationToken)
        {
            var tools = await _toolRepository.GetAllAsync();
            return _mapper.Map<List<ToolDto>>(tools);
        }
    }
}
