using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System.Net;

namespace MobileProgramming.Business.UseCase.Notification.Commands.CreateNotification
{
    public class CreateNotificationHandler : IRequestHandler<CreateNotification, APIResponse>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateNotificationHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(CreateNotification request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = "User not found."
                };
            }

            var notification = new Data.Entities.Notification
            {
                UserId = request.UserId,
                Message = request.Message ?? string.Empty,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            await _notificationRepository.Add(notification);
            await _unitOfWork.SaveChangesAsync();

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.Created,
                Message = "Notification created successfully."
            };
        }
    }
}
