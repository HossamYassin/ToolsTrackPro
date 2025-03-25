namespace ToolsTrackPro.API.Hub
{
    using Microsoft.AspNetCore.SignalR; 

    public class ToolNotificationHub : Hub
    {
        public async Task NotifyToolAvailable(string toolId)
        {
            await Clients.All.SendAsync("ToolAvailable", toolId);
        }
    }
}
