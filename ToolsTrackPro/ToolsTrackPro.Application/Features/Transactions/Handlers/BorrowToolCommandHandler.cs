using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Application.Features.Transactions.Commands;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Transactions.Handlers
{
    public class BorrowToolCommandHandler : IRequestHandler<BorrowToolCommand, bool>
    {
        private readonly ITransactionRepository _transactionRepository;

        public BorrowToolCommandHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<bool> Handle(BorrowToolCommand request, CancellationToken cancellationToken)
        {
            return await _transactionRepository.BorrowToolAsync(request.UserId, request.ToolId, request.BorrowDate, request.DueDate);
        }
    }
}
