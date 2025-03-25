using AutoMapper;
using MediatR;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Application.Features.Tools.Handlers
{
    public class GetNotificationsByUserIdQueryHandler : IRequestHandler<GetNotificationsByUserIdQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetNotificationsByUserIdQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<List<NotificationDto>> Handle(GetNotificationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetUserNotificationsAsync(request.UserId);
            return _mapper.Map<List<NotificationDto>>(notifications);
        }
    }
}
