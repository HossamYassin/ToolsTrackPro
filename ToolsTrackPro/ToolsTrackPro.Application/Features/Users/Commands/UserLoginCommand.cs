using MediatR;
using System.ComponentModel.DataAnnotations;
using ToolsTrackPro.Application.DTOs;

namespace ToolsTrackPro.Application.Features.Users.Commands
{
    public class UserLoginCommand : IRequest<UserDto?>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
