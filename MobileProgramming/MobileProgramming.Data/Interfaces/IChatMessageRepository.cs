using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Interfaces;

public interface IChatMessageRepository: IRepository<ChatMessage>
{
    Task<List<ChatMessage>> GetChatHistory(int senderId, int? recepientId);
    Task<List<ChatMessage>> GetUserChatHistory(int userId, DateTime? filter);
    Task<List<List<ChatMessage>>> GetAdminChatHistory(int userId, int page, int pageSize);
}
