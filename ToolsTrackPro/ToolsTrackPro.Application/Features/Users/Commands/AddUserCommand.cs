using MediatR;
using ToolsTrackPro.Application.DTOs;

namespace ToolsTrackPro.Application.Features.Users.Commands
{
    public class AddUserCommand : IRequest<bool>
    {
        public UserDto User { get; }

        public AddUserCommand(UserDto user) => User = user;
    }
}
