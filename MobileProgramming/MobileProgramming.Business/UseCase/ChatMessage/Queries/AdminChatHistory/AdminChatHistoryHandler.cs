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
        List<List<ChatDto>> response = new List<List<ChatDto>>();
        var chatMessages = await _chatMessageRepository.GetAdminChatHistory(request.UserId, request.Page, request.PageSize);
        if (chatMessages.Any())
        {
            foreach (var chatMessage in chatMessages)
            {
                List<ChatDto> temp = await toDto(chatMessage);
                response.Add(temp);
            }
            
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
    private async Task<List<ChatDto>> toDto(List<ChatMessage> chatMessages)
    {
        List<ChatDto> response = new List<ChatDto>();
        foreach (ChatMessage chatMessage in chatMessages)
        {
            ChatDto dto = new ChatDto();
            dto.ChatMessageId = chatMessage.ChatMessageId;
            dto.Message = chatMessage.Message;
            dto.SentAt = chatMessage.SentAt;
            dto.SendFrom = _mapper.Map<UserInfoDto>(await _userRepo.GetById(chatMessage.UserId!));
            dto.SendTo = _mapper.Map<UserInfoDto>(await _userRepo.GetById(chatMessage.SendTo));
            response.Add(dto);
        }
        return response;
    }
}
