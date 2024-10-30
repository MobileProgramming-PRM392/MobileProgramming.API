using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.API.Helper;
using MobileProgramming.Business.Models.DTO.User;
using MobileProgramming.Business.UseCase;
using MobileProgramming.Business.UseCase.Authentication.Command.Register;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MobileProgramming.API.Controllers
{
    [Route("api/v1/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ISender _mediator;


        public CartController(ISender mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("")]
        public async Task<IActionResult> Register(CancellationToken token = default)
        {
            var userIdString = User.GetUserIdFromToken();
            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID."); // Trả về lỗi nếu không thể chuyển đổi
            }

            var result = await _mediator.Send(new GetCartByUserIdQuery(userId), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }
    }
}
