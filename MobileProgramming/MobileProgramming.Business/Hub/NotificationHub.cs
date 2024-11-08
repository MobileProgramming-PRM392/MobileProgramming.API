using Microsoft.AspNetCore.SignalR;

namespace MobileProgramming.Business.Hubs
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub<INotificationClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification($"Thank you connecting {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }
    }

    public interface INotificationClient
    {
        Task ReceiveNotification(object message);
    }
}
