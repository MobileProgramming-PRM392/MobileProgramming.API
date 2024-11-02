using Microsoft.AspNetCore.SignalR;

namespace MobileProgramming.Business.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        //public async Task SendNotification(string userId, object message)
        //{
        //    await Clients.User(userId).SendAsync("ReceiveNotification", message);
        //}
    }
}
