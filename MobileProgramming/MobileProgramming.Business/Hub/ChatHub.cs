using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using MobileProgramming.Business.Models.DTO.Chat;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using Newtonsoft.Json;

namespace MobileProgramming.Business.Hub;



public interface IChatHub
{
    Task SendChatMessages(ChatDto dto);
}
public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
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
        await Clients.All.SendAsync("connection check", $"connection estabished: {Context.ConnectionId}");
    }
    public async Task SendMessageChatTest(string messageDto)
    {
        //SendMessageDto  dto = JsonConvert.DeserializeObject<SendMessageDto>(messageDto);
        ChatMessage message = new ChatMessage();
        string[] temp = messageDto.Split(',');
        message.UserId = int.Parse(temp[0]);
        message.SendTo = int.Parse(temp[1]);
        message.Message = temp[2];
        await Groups.AddToGroupAsync(Context.ConnectionId, temp[3]);
        await _messageRepository.Add(message);
        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            
            ChatDto chatMessage = new ChatDto
            {
                ChatMessageId = message.ChatMessageId,
                SenderId = message.UserId.Value,
                ReceiverId = message.SendTo,
                Message = message.Message,
                SentAt = message.SentAt
            };

            await Clients.All.SendAsync("Received-message", JsonConvert.SerializeObject(chatMessage));
        }
    }
    public async Task SendPrivateMessageChatTest(string messageDto)
    {
        //SendMessageDto  dto = JsonConvert.DeserializeObject<SendMessageDto>(messageDto);
        ChatMessage message = new ChatMessage();
        string[] temp = messageDto.Split(',');
        message.UserId = int.Parse(temp[0]);
        message.SendTo = int.Parse(temp[1]);
        message.Message = temp[2];
        await Groups.AddToGroupAsync(Context.ConnectionId, temp[3]);
        await _messageRepository.Add(message);
        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            
            ChatDto chatMessage = new ChatDto
            {
                ChatMessageId = message.ChatMessageId,
                SenderId = message.UserId.Value,
                ReceiverId = message.SendTo,
                Message = message.Message,
                SentAt = message.SentAt
            };
            await Clients.Group(temp[3]).SendAsync("Received-message", JsonConvert.SerializeObject(chatMessage));
        }
    }
    public async Task SendMessageChat(SendMessageDto dto)
    {
        var message = CreateChatMessage(dto);
        //await Groups.AddToGroupAsync(Context.ConnectionId, dto.RoomNo.ToString());
        await _messageRepository.Add(message);

        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            var sendFrom = _mapper.Map<UserInfoDto>(await _userRepo.GetById(message.UserId!));
            var chatMessage = CreateChatDto(message, sendFrom);
            await Clients.All.SendAsync("Received-message", chatMessage/*JsonConvert.SerializeObject(chatMessage)*/);
        }
    }

    //public async Task SendPrivateMessageChat(SendMessageDto dto)
    //{
    //    var message = CreateChatMessage(dto);
    //    await Groups.AddToGroupAsync(Context.ConnectionId, dto.RoomNo.ToString());
    //    await _messageRepository.Add(message);

    //    if (await _unitOfWork.SaveChangesAsync() > 0)
    //    {
    //        var sendFrom = _mapper.Map<UserInfoDto>(await _userRepo.GetById(message.UserId!));
    //        var chatMessage = await CreateChatDto(message, sendFrom);
    //        await Clients.Group(dto.RoomNo.ToString()).SendAsync("Received-message", chatMessage *//*JsonConvert.SerializeObject(chatMessage)*//*);
    //    }
    //}

    private ChatMessage CreateChatMessage(SendMessageDto dto)
    {
        return new ChatMessage
        {
            UserId = dto.UserId,
            SendTo = dto.ReceiverId,
            Message = dto.Message
        };
    }
    private ChatDto CreateChatDto(ChatMessage message, UserInfoDto sendFrom)
    {
        return new ChatDto
        {
            ConversationId = "",
            ChatMessageId = message.ChatMessageId,
            SenderId = sendFrom.UserId,
            ReceiverId = message.SendTo,
            Message = message.Message,
            SentAt = message.SentAt,
        };
    }
    public async Task SendMessageTest(string message)
    {
        await Clients.All.SendAsync("Received-message", message);
    }
}
