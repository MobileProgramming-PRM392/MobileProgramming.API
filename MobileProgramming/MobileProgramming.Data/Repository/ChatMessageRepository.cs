using Microsoft.EntityFrameworkCore;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Persistence;
using System.Drawing.Printing;

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
    public async Task<List<ChatMessage>> GetUserChatHistory(int userId, DateTime? filter)
    {
        var userMessage = await _context.ChatMessages
        .Where(c => c.UserId == userId || c.SendTo == userId)
        .ToListAsync();
        if (filter.HasValue)
        {
            userMessage = userMessage.Where(u => u.SentAt >= filter.Value).ToList();
        }
        return userMessage.OrderByDescending(p => p.SentAt).ToList();
    }
    /*public async Task<Dictionary<int, ChatMessage>> GetAdminChatHistory(int userId)
    {
        
        var adminMessage = await _context.ChatMessages.Where(c => c.UserId == userId).ToListAsync();
        List<int> sendTo = adminMessage.Select(p => p.SendTo).Distinct().ToList();
        var groupedMessages = adminMessage
        .GroupBy(msg => msg.SendTo)
        .OrderByDescending(g => g.Count())
        .Select(g => g.First())
        .ToDictionary(g => g.SendTo, g => g);

        return groupedMessages;
    }*/
    public async Task<List<List<ChatMessage>>> GetAdminChatHistory(int userId, int page, int pageSize)
    {

        var adminMessage = await _context.ChatMessages.Where(c => c.UserId == userId).ToListAsync();
        List<int> sendTo = adminMessage.OrderByDescending(p => p.SentAt).Select(p => p.SendTo).Distinct().ToList();

        List<List<ChatMessage>> response = new List<List<ChatMessage>>();
        foreach(int id in sendTo){
            List<ChatMessage> tmp = adminMessage.Where(c => c.SendTo == id).ToList();
            List<ChatMessage> res = _context.ChatMessages.Where(c => c.UserId == id && c.SendTo == userId).ToList();
            tmp.AddRange(res);
            tmp = tmp.OrderByDescending(p => p.SentAt).ToList();
            response.Add(tmp);
        }

        return response;
    }
}
