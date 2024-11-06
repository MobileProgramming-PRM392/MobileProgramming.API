using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.ChatMessages.Queries.AdminChatHistory;

public class AdminChatHistoryCommand: IRequest<APIResponse>
{
    public int UserId { get; set; }
    public int Page {  get; set; }
    public int PageSize { get; set; }
    public AdminChatHistoryCommand(int userId, int page, int pageSize)
    {
        UserId = userId;
        Page = page;
        PageSize = pageSize;
    }
}
