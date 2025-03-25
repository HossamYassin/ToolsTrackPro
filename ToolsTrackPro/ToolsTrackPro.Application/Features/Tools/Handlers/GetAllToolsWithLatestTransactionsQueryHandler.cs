using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Tools.Queries;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Tools.Handlers
{
    public class GetAllToolsWithLatestTransactionsQueryHandler : IRequestHandler<GetAllToolsWithLatestTransactionsQuery, List<ToolTransactionDto>>
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;

        public GetAllToolsWithLatestTransactionsQueryHandler(IToolRepository toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        public async Task<List<ToolTransactionDto>> Handle(GetAllToolsWithLatestTransactionsQuery request, CancellationToken cancellationToken)
        {
            var tools = await _toolRepository.GetToolsWithLatestTransactionAsync();
            return _mapper.Map<List<ToolTransactionDto>>(tools);
        }
    }
}
