using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Tools.Handlers
{
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.MarkNotificationAsReadAsync(request.Id);
        }
    }
}
