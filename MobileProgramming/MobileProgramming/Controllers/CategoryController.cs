using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.UseCase.Categories.Command.Create;
using MobileProgramming.Business.UseCase.Categories.Command.Delete;
using MobileProgramming.Business.UseCase.Categories.Queries.GetAllCategory;
using System.ComponentModel.DataAnnotations;
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

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory([FromQuery,Required] int categoryId, CancellationToken token = default)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(categoryId), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromQuery, Required] string category, CancellationToken token = default)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(category), token);
            return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
        }
    }
}
