using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Notification.Commands.UpdateNotification
{
    public class UpdateNotification : IRequest<APIResponse>
    {
        public int NotificationId { get; set; }

        public UpdateNotification(int notificationId)
        {
            NotificationId = notificationId;
        }
        //public int UserId { get; set; }

    }
}
