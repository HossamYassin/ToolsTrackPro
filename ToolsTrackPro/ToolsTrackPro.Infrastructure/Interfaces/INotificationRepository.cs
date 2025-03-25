
namespace ToolsTrackPro.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ToolsTrackPro.Domain.Entities;

    public interface INotificationRepository
    {
        Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
    }

}
