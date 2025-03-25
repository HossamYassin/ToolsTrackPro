using MediatR;
using ToolsTrackPro.Application.DTOs;

namespace ToolsTrackPro.Application.Features.Tools.Queries
{
    public class GetAllToolsWithLatestTransactionsQuery : IRequest<List<ToolTransactionDto>>
    { 

    }
}
