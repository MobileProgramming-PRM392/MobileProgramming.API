using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.ChatMessages.Queries.GetChatHistory;

public class GetChatHistoryQuery:IRequest<APIResponse>
{
    public int SenderId { get; set; }
    public int? RecepientId { get; set; }

    public GetChatHistoryQuery(int senderId, int? recepientId)
    {
        SenderId = senderId;
        RecepientId = recepientId;
    }
}
