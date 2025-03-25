using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.Features.Users.Commands;
using ToolsTrackPro.Application.Utilities;
using ToolsTrackPro.Domain.Entities;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Users.Handlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AddUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<User>(request.User);
            entity.PasswordHash = PasswordHelper.HashPassword(request.User.Password);

            return await _userRepository.CreateUserAsync(entity);
        }
    }
}
