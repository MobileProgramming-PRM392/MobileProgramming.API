using MediatR;
using MobileProgramming.Business.Models.DTO.Notification;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase.Notification.Queries.GetFilterNotification
{
    public class GetFilterNotificationHandler : IRequestHandler<GetFilterNotification, APIResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetFilterNotificationHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<APIResponse> Handle(GetFilterNotification request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetNotificationsByFilter(request.UserId, request.IsRead);

            if (!notifications.Any())
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "No notifications found."
                };
            }

            var notificationDTOs = notifications.Select(n => new NotificationDto(
                notificationId: n.NotificationId,
                UserId: (int)n.UserId!,
                Message: n.Message!,
                IsRead: n.IsRead,
                CreatedAt: n.CreatedAt
            )).ToList();

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Notifications retrieved successfully.",
                Data = notificationDTOs
            };
        }
    }
}
