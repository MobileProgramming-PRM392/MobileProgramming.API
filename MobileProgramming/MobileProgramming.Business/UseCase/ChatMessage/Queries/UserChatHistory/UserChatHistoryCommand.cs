using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.ChatMessages.Queries.UserChatHistory;

public class UserChatHistoryCommand : IRequest<APIResponse>
{
    public int UserId { get; set; }
    public DateTime? Filter {  get; set; }
    public UserChatHistoryCommand(int userId, DateTime? filter)
    {
        UserId = userId;
        Filter = filter;
    }
}
