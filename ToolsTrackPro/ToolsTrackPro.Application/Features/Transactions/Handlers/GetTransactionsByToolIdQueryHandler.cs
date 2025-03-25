using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Transactions.Queries;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Transactions.Handlers
{
    internal class GetTransactionsByToolIdQueryHandler : IRequestHandler<GetTransactionsByToolIdQuery, List<TransactionViewDto>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetTransactionsByToolIdQueryHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<List<TransactionViewDto>> Handle(GetTransactionsByToolIdQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetTransactionsByToolIdAsync(request.ToolId);
            return _mapper.Map<List<TransactionViewDto>>(transactions);
        }
    }
}
