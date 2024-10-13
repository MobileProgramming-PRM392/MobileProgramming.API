using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.UseCase.Products.Queries.GetAllProducts;
using MobileProgramming.Business.UseCase.Products.Queries.GetFilteredProducts;
using MobileProgramming.Data.Models.Product;
using System.Net;

namespace MobileProgramming.API.Controllers
{
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ISender _mediator;


        public ProductController(ISender mediator)
        {
            _mediator = mediator;

        }

        //[Authorize]
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetAllProduct(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }


        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetFilteredProduct([FromQuery] ProductFilterDto filter, [FromQuery] ProductSortDto sort , CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetFilteredProductsQuery(filter, sort), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }
    }
}
