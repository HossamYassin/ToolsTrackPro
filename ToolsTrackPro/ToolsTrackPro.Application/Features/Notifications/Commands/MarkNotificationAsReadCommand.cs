using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsTrackPro.Application.DTOs;

namespace ToolsTrackPro.Application.Features.Tools.Commands
{
    public class MarkNotificationAsReadCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
