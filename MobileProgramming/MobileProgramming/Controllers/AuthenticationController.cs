using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.Business.Models.DTO.User;
using MobileProgramming.Business.UseCase.Authentication.Command.Login;
using MobileProgramming.Business.UseCase.Authentication.Command.Register;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MobileProgramming.API.Controllers;

[Route("api/v1/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private ISender _mediator;


    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;

    }
    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody, Required] RegisterUserDto user, CancellationToken token = default)
    {
        var result = await _mediator.Send(new RegisterCommand(user), token);
        return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
    }
    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody, Required] LoginDto user, CancellationToken token = default)
    {
        var result = await _mediator.Send(new LoginCommand(user), token);
        return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
    }
}
