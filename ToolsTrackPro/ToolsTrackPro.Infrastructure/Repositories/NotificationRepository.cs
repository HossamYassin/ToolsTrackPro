namespace ToolsTrackPro.Infrastructure.Repositories
{
    using Microsoft.Data.SqlClient;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ToolsTrackPro.Domain.Entities;
    using ToolsTrackPro.Infrastructure.Interfaces;

    public class NotificationRepository : INotificationRepository
    {
        private readonly string _connectionString;

        public NotificationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Fetch notifications for a specific user.
        /// </summary>
        public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
        {
            var notifications = new List<Notification>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("SELECT ID, UserID, Message, CreatedAt, IsRead FROM Notifications WHERE UserID = @UserID ORDER BY CreatedAt DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            notifications.Add(new Notification
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                Message = reader.GetString(2),
                                CreatedAt = reader.GetDateTime(3),
                                IsRead = reader.GetBoolean(4)
                            });
                        }
                    }
                }
            }

            return notifications;
        }

        /// <summary>
        /// Mark a notification as read.
        /// </summary>
        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("UPDATE Notifications SET IsRead = 1 WHERE ID = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", notificationId);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }

}
