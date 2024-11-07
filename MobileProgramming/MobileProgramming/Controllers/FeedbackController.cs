using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.API.Helper;
using MobileProgramming.Business.Models.DTO.Feedbacks;
using MobileProgramming.Business.UseCase;
using MobileProgramming.Business.UseCase.Feedbacks.Command.Create;
using MobileProgramming.Business.UseCase.Feedbacks.Command.Delete;
using MobileProgramming.Business.UseCase.Feedbacks.Command.Update;
using MobileProgramming.Business.UseCase.Feedbacks.Queries.GetAllFeedbacksByProductId;
using MobileProgramming.Business.UseCase.Feedbacks.Queries.GetMyFeedbackByProductId;
using System.ComponentModel.DataAnnotations;
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

        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody, Required] CreateFeedbackDto dto, CancellationToken token = default)
        {
            var userIdString = User.GetUserIdFromToken();
            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID."); // Trả về lỗi nếu không thể chuyển đổi
            }
            dto.UserId = userId;
            var result = await _mediator.Send(new CreateFeedbackCommand(dto), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFeedback([FromQuery, Required] int id, CancellationToken token = default)
        {
            var result = await _mediator.Send(new DeleteFeedbackCommand(id), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeedback([FromQuery, Required] int id, [FromBody, Required] UpdateFeedbackDto dto, CancellationToken token = default)
        {
            var result = await _mediator.Send(new UpdateFeedbackCommand(id, dto), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }
    }
}
