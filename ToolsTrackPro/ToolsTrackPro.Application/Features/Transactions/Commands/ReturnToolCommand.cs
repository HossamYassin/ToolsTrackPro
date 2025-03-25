using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsTrackPro.Application.Features.Transactions.Commands
{
    public class ReturnToolCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int ToolId { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
