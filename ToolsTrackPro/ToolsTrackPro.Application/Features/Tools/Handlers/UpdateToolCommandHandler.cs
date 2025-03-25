using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Domain.Entities;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Tools.Handlers
{
    public class UpdateToolCommandHandler : IRequestHandler<UpdateToolCommand, bool>
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;

        public UpdateToolCommandHandler(IToolRepository toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateToolCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Tool>(request.Tool);
            return await _toolRepository.UpdateAsync(entity);
        }
    }
}
