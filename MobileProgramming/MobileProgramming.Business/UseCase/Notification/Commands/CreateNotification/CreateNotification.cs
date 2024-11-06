using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Notification.Commands.CreateNotification
{
    public class CreateNotification : IRequest<APIResponse>
    {
        public int UserId { get; set; }

        public string? Message { get; set; }
    }
}
