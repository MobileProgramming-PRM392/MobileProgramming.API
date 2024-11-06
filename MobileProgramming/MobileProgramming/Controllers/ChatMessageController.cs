using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProgramming.API.Helper;
using MobileProgramming.Business.UseCase.ChatMessages.Queries.AdminChatHistory;
using MobileProgramming.Business.UseCase.ChatMessages.Queries.GetChatHistory;
using MobileProgramming.Business.UseCase.ChatMessages.Queries.UserChatHistory;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace MobileProgramming.API.Controllers;

[Route("api/v1/chat")]
[ApiController]
public class ChatMessageController : ControllerBase
{
    private ISender _mediator;


    public ChatMessageController(ISender mediator)
    {
        _mediator = mediator;

    }

    [Authorize]
    [HttpGet("history")]
    public async Task<IActionResult> GetChatHistory([FromQuery, AllowNull] int? recepientId, CancellationToken token = default)
    {
        int userId = int.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new GetChatHistoryQuery(userId, recepientId), token);
        return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
    }
    [Authorize]
    [HttpGet("history/user")]
    public async Task<IActionResult> GetUserChatHistory([FromQuery, AllowNull] DateTime? Filter, CancellationToken token = default)
    {
        int userId = int.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new UserChatHistoryCommand(userId, Filter), token);
        return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
    }
    [Authorize(Roles ="Admin")]
    [HttpGet("history/admin")]
    public async Task<IActionResult> GetAdminChatHistory([FromQuery] int page = 1, 
                                                         [FromQuery] int pageSize = 10, 
                                                         CancellationToken token = default)
    {
        int userId = int.Parse(User.GetUserIdFromToken());
        var result = await _mediator.Send(new AdminChatHistoryCommand(userId, page, pageSize), token);
        return (result.StatusResponse != HttpStatusCode.OK) ? Ok(result) : StatusCode((int)result.StatusResponse, result);
    }
}
