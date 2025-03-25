using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Tools.Handlers
{
    public class DeleteToolCommandHandler : IRequestHandler<DeleteToolCommand, bool>
    {
        private readonly IToolRepository _toolRepository;

        public DeleteToolCommandHandler(IToolRepository toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
        }

        public async Task<bool> Handle(DeleteToolCommand request, CancellationToken cancellationToken)
        {
            return await _toolRepository.DeleteAsync(request.Id);
        }
    }
}
