using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using MobileProgramming.Business.Models.DTO.Chat;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using Newtonsoft.Json;

namespace MobileProgramming.API.Hubs;



public interface IChatHub
{
    Task SendChatMessages(ChatDto dto);
}
public class ChatHub: Hub
{
    private readonly IChatMessageRepository _messageRepository;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public ChatHub(IChatMessageRepository messageRepository, IUserRepository userRepo, 
        IMapper mapper, IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository;
        _userRepo = userRepo;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("connection check",$"connection estabished: {Context.ConnectionId}");
    }
    public async Task SendMessageChat(string messageDto)
    {
        //SendMessageDto  dto = JsonConvert.DeserializeObject<SendMessageDto>(messageDto);
        ChatMessage message = new ChatMessage();
        string[] temp = messageDto.Split(',');
        message.UserId = int.Parse(temp[0]);
        message.SendTo = int.Parse(temp[1]);
        message.Message = temp[2];
        await Groups.AddToGroupAsync(Context.ConnectionId, temp[3]);
        await _messageRepository.Add(message);
        if(await _unitOfWork.SaveChangesAsync() > 0)
        {
            UserInfoDto sendFrom = _mapper.Map<UserInfoDto>(await _userRepo.GetById(message.UserId!));
            UserInfoDto sendTo = _mapper.Map<UserInfoDto>(await _userRepo.GetById(message.SendTo));
            ChatDto chatMessage = new ChatDto
            {
                ChatMessageId = message.ChatMessageId,
                SendFrom = sendFrom,
                SendTo = sendTo,
                Message = message.Message,
                SentAt = message.SentAt,
            };
            if (sendFrom.Role.Equals("Admin"))
            {
                await Clients.Group(temp[3]).SendAsync("Received-message", JsonConvert.SerializeObject(chatMessage));
            }
            await Clients.All.SendAsync("Received-message", JsonConvert.SerializeObject(chatMessage));
            
        }
    }
    public async Task SendMessageTest(string message)
    {
        await Clients.All.SendAsync("Received-message", message);
    }
}
