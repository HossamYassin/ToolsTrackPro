using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Users.Commands;
using ToolsTrackPro.Application.Utilities;
using ToolsTrackPro.Domain.Entities;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Users.Handlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserLoginCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user != null && PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return  _mapper.Map<UserDto>(user); ;
            }
            return null;
        }
    }
}
