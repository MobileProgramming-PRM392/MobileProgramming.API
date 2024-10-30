using Microsoft.EntityFrameworkCore;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Persistence;

namespace MobileProgramming.Data.Repository;

public class ChatMessageRepository : RepositoryBase<ChatMessage>, IChatMessageRepository
{
    private readonly SaleProductDbContext _context;
    public ChatMessageRepository(SaleProductDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ChatMessage>> GetChatHistory(int senderId, int? recepientId)
    {
        var chatHistory = _context.ChatMessages.AsQueryable().Where(p => p.UserId == senderId);
        var chatHistory2 = new List<ChatMessage>();
        if (recepientId.HasValue)
        {
            chatHistory = chatHistory.Where(p => p.SendTo == recepientId.Value);
            chatHistory2 = await _context.ChatMessages.AsQueryable().Where(p => p.UserId == recepientId && p.SendTo == senderId).ToListAsync();
        }
        else
        {
            var firstChat = await chatHistory.FirstOrDefaultAsync();
            int sendTo = firstChat?.SendTo ?? 0;

            if (sendTo > 0)
            {
                chatHistory = chatHistory.Where(p => p.SendTo == sendTo);
            }
            chatHistory2 = await _context.ChatMessages.AsQueryable().Where(p => p.UserId == sendTo && p.SendTo == senderId).ToListAsync();
        }
        var response = await chatHistory.ToListAsync();
        response.AddRange(chatHistory2);
        return response.OrderBy(p => p.SentAt).ToList();
    }
    /*public async Task<ChatMessage?> GetChatDetail(int id)
{
   return await _context.ChatMessages.Include(c => c.User).FirstOrDefaultAsync(c => c.ChatMessageId == id);
}*/
}
