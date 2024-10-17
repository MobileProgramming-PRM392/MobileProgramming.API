﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.UseCase;
using MobileProgramming.Business.UseCase.Products.Queries.GetFilteredProducts;
using MobileProgramming.Data.Models.Product;
using System.Net;

namespace MobileProgramming.API.Controllers
{
    [Route("api/v1/cartitem")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private ISender _mediator;


        public CartItemController(ISender mediator)
        {
            _mediator = mediator;

        }


        [HttpPost("add-cartItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> AddCartItem([FromBody] int userId, int productId, int quantity, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new AddCartItemCommand(userId, productId, quantity), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }
    }
}