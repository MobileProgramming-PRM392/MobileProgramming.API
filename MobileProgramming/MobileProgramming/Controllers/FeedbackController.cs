using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.API.Helper;
using MobileProgramming.Business.UseCase;
using MobileProgramming.Business.UseCase.Feedbacks.Queries.GetAllFeedbacksByProductId;
using MobileProgramming.Business.UseCase.Feedbacks.Queries.GetMyFeedbackByProductId;
using System.Net;

namespace MobileProgramming.API.Controllers
{
    [Route("api/v1/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {

        private ISender _mediator;


        public FeedbackController(ISender mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("productId")]
        public async Task<IActionResult> GetFeedbackByProductId([FromQuery] int productId, CancellationToken token = default)
        {
            

            var result = await _mediator.Send(new GetFeedbackByProductIdQuery(productId), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }

        [HttpGet("productId-mine")]
        public async Task<IActionResult> GetMyFeedbackByProductId([FromQuery] int productId, CancellationToken token = default)
        {
            var userIdString = User.GetUserIdFromToken();
            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID."); // Trả về lỗi nếu không thể chuyển đổi
            }

            var result = await _mediator.Send(new GetMyFeedbackByProductIdQuery(productId, userId), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }
    }
}
