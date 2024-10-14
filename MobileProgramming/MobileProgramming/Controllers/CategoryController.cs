using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.UseCase.Category.Queries.GetAllCategory;
using MobileProgramming.Business.UseCase.Products.Queries.GetAllProducts;
using System.Net;

namespace MobileProgramming.API.Controllers
{
    [Route("api/v1/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ISender _mediator;


        public CategoryController(ISender mediator)
        {
            _mediator = mediator;

        }


        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllCategory(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllCategoryQuery(), cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);
        }
    }
}
