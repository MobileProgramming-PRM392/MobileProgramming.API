using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Notification.Commands.UpdateNotification
{
    public class UpdateNotification : IRequest<APIResponse>
    {
        public int NotificationId { get; set; }
        //public int UserId { get; set; }
    }
}
