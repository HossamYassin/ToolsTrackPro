using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsTrackPro.Application.Features.Tools.Commands
{
    public class DeleteToolCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
