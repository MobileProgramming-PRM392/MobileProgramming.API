using Microsoft.AspNetCore.SignalR;
using MobileProgramming.Data.Interfaces;

namespace MobileProgramming.API.Hub;



public interface IChatHub
{
    Task SendMessage(int userId, string message);
}
public class ChatHub: Hub<IChatHub>
{
    private readonly IChatMessageRepository _messageRepository;

    public ChatHub(IChatMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
}
