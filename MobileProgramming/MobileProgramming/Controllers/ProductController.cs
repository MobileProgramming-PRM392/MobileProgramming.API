using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.UseCase.Products.Command.CreateProduct;
using MobileProgramming.Business.UseCase.Products.Command.UploadProducImages;
using MobileProgramming.Business.UseCase.Products.Queries.GetAllProducts;
using MobileProgramming.Business.UseCase.Products.Queries.GetFilteredProducts;
using MobileProgramming.Business.UseCase.Products.Queries.GetProductDetail;
using MobileProgramming.Data.Models.Product;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;

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
        // cíu bé
        //[Authorize]
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetAllProduct(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }

        [HttpGet("product-detail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetProductDetail([FromQuery] int productId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetProductDetailQuery(productId), cancellationToken);
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
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody, Required] CreateProductDto dto, CancellationToken token= default)
        {
            var result = await _mediator.Send(new CreateProductCommand(dto), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromBody, Required] List<ProductImageDto> images,[FromQuery, Required] int productId, 
            CancellationToken token= default)
        {
            var result = await _mediator.Send(new UploadProducImagesCommand(productId, images), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }
    }
}
