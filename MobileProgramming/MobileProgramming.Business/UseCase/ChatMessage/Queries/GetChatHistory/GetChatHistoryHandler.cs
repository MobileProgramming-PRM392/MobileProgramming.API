using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Chat;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;

namespace MobileProgramming.Business.UseCase.ChatMessages.Queries.GetChatHistory;

internal class GetChatHistoryHandler : IRequestHandler<GetChatHistoryQuery, APIResponse>
{
    private readonly IMapper _mapper;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IUserRepository _userRepo;

    public GetChatHistoryHandler(IMapper mapper, IChatMessageRepository chatMessageRepository, IUserRepository userRepository)
    {
        _mapper = mapper;
        _chatMessageRepository = chatMessageRepository;
        _userRepo = userRepository;
    }

    public async Task<APIResponse> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        List<ChatMessage> chatMessages = await _chatMessageRepository.GetChatHistory(request.SenderId, request.RecepientId);
        if(chatMessages.Any())
        {
            List<ChatDto> response = await toDto(chatMessages);
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = response
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
