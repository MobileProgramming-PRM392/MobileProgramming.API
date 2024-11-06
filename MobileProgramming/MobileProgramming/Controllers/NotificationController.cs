using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.Business.UseCase.Notification.Commands.UpdateNotification;
using MobileProgramming.Business.UseCase.Notification.Queries.GetFilterNotification;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MobileProgramming.API.Controllers
{
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly ISender _mediator;

        public NotificationController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetFilteredNotifications([FromQuery, Required] int userId, [FromQuery] bool? isRead, CancellationToken token = default)
        {
            var query = new GetFilterNotification
            {
                UserId = userId,
                IsRead = isRead
            };

            var result = await _mediator.Send(query, token);
            return (result.StatusResponse == HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }

        [HttpPut("mark-as-read")]
        public async Task<IActionResult> Update([FromQuery, Required] int notificationId, CancellationToken token = default)
        {
            var result = await _mediator.Send(new UpdateNotification(notificationId), token);
            return (result.StatusResponse == HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }
    }
}
