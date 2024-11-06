using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces.Common;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase.Notification.Commands.UpdateNotification
{
    public class UpdateNotificationHandler : IRequestHandler<UpdateNotification, APIResponse>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateNotificationHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(UpdateNotification request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetById(request.NotificationId);

            if (notification == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "Notification not found."
                };
            }

           
            notification.IsRead = true;
            await _notificationRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync();

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Notification updated successfully."
            };
        }
    }
}
