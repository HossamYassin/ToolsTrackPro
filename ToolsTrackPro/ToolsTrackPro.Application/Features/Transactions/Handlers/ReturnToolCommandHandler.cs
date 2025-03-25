using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Application.Features.Transactions.Commands;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Transactions.Handlers
{
    public class ReturnToolCommandHandler : IRequestHandler<ReturnToolCommand, bool>
    {
        private readonly ITransactionRepository _transactionRepository;

        public ReturnToolCommandHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<bool> Handle(ReturnToolCommand request, CancellationToken cancellationToken)
        {
            return await _transactionRepository.ReturnToolAsync(request.UserId, request.ToolId, request.ReturnDate);
        }
    }
}
