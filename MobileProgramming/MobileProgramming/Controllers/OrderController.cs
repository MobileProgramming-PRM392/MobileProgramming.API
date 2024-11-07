using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.Business.UseCase.Order.Commands.CreateOrder;
using MobileProgramming.Business.UseCase.Order.Queries.GetOrder;
using MobileProgramming.Business.UseCase.Order.Queries.QueryOrder;
using MobileProgramming.Business.UseCase.Payment.Query.GetFilterPayment;
using System.Net;

namespace MobileProgramming.API.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly ISender _mediator;

        public OrderController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken token = default)
        {
            var result = await _mediator.Send(command, token);
            return (result.StatusResponse == HttpStatusCode.Created) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOrder([FromQuery] GetOrderQuery query, CancellationToken token = default)
        {
            var result = await _mediator.Send(query, token);
            return (result.StatusResponse == HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }

        [HttpGet("billing")]
        public async Task<IActionResult> GetPayment([FromQuery] GetFilterPaymentQuery query, CancellationToken token = default)
        {
            var result = await _mediator.Send(query, token);
            return (result.StatusResponse == HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }

        [HttpGet("query")]
        public async Task<IActionResult> QueryOrder([FromQuery] string orderId, CancellationToken token = default)
        {
            var query = new QueryOrder(orderId);
            var result = await _mediator.Send(query, token);
            return (result.StatusResponse == HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }
    }
}

    
