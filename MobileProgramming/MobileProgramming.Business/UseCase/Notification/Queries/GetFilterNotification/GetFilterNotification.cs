using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Notification.Queries.GetFilterNotification
{
    public class GetFilterNotification : IRequest<APIResponse>
    {
        public int UserId { get; set; }
        public bool? IsRead { get; set; }
    }
}
