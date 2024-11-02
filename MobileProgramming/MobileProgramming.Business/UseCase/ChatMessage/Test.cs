using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.ChatMessages;

public class Test : IRequestHandler<TestCommand, APIResponse>
{
    private readonly IChatMessageRepository _chatMessageRepository;

    public Test(IChatMessageRepository chatMessageRepository)
    {
        _chatMessageRepository = chatMessageRepository;
    }
    public async Task<string> GetChatMessage()
    {
        ChatMessage entity = await _chatMessageRepository.GetById(1);
        return $"message from {entity.UserId} to {entity.SendTo} at {entity.SentAt}:  {entity.Message}";
    }

    async Task<APIResponse> IRequestHandler<TestCommand, APIResponse>.Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return new APIResponse
        {
            Data = await GetChatMessage(),
            StatusResponse = System.Net.HttpStatusCode.OK,
            Message = MessageCommon.Complete
        };
    }
}
