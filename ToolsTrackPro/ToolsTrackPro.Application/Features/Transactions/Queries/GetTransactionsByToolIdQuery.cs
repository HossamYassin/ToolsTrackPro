using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsTrackPro.Application.DTOs;

namespace ToolsTrackPro.Application.Features.Transactions.Queries
{
    public class GetTransactionsByToolIdQuery : IRequest<List<TransactionViewDto>>
    {
        public int ToolId { get; set; }
    }
}
