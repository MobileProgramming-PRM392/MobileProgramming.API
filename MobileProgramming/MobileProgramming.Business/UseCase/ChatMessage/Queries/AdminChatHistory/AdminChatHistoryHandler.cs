using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Chat;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.ChatMessages.Queries.AdminChatHistory;

public class AdminChatHistoryHandler : IRequestHandler<AdminChatHistoryCommand, APIResponse>
{
    private readonly IMapper _mapper;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IUserRepository _userRepo;

    public AdminChatHistoryHandler(IMapper mapper, IChatMessageRepository chatMessageRepository, IUserRepository userRepository)
    {
        _mapper = mapper;
        _chatMessageRepository = chatMessageRepository;
        _userRepo = userRepository;
    }
    public async Task<APIResponse> Handle(AdminChatHistoryCommand request, CancellationToken cancellationToken)
    {
        List<ConversationDto> response = new List<ConversationDto>();
        var chatMessages = await _chatMessageRepository.GetAdminChatHistory(request.UserId, request.Page, request.PageSize);
        if (chatMessages.Any())
        {
            foreach (var chatMessage in chatMessages)
            {
                ConversationDto temp = new ConversationDto();
                ChatMessage message = chatMessage.FirstOrDefault()!;
                User? user = await _userRepo.GetById(message.UserId!);
                User? user2 = await _userRepo.GetById(message.SendTo!);
                UserInfoDto participant = _mapper.Map<UserInfoDto>(user);
                UserInfoDto participant2 = _mapper.Map<UserInfoDto>(user2);
                
                temp.Participants.Add(participant);
                temp.Participants.Add(participant2);
                string conversationId = "";
                if (user!.Role == "Admin")
                {
                    conversationId = conversationId + $"{participant.Username}-{participant2.Username}";
                }
                else
                {
                    conversationId = conversationId + $"{participant2.Username}-{participant.Username}";
                }
                temp.ConversationId = conversationId;
                temp.Chats = toDto(chatMessage, conversationId);
                temp.LastMessageTimestamp = temp.Chats.FirstOrDefault()!.SentAt;
                response.Add(temp);
            }
            response = response.OrderByDescending(r => r.LastMessageTimestamp).ToList();
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = response.AsQueryable().Pagination(request.Page, request.PageSize)
            };
        }
        return new APIResponse
        {
            StatusResponse = System.Net.HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = new List<ChatMessage>()
        };
    }
    private List<ChatDto> toDto(List<ChatMessage> chatMessages, string conversationId)
    {
        List<ChatDto> response = new List<ChatDto>();
        foreach (ChatMessage chatMessage in chatMessages)
        {
            ChatDto dto = new ChatDto();
            dto.ConversationId = conversationId;
            dto.ChatMessageId = chatMessage.ChatMessageId;
            dto.Message = chatMessage.Message;
            dto.SentAt = chatMessage.SentAt;
            dto.SenderId = chatMessage.UserId.Value;
            response.Add(dto);
        }
        return response;
    }
}
