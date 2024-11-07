using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Command.Delete
{
    public class DeleteFeedbackHandler : IRequestHandler<DeleteFeedbackCommand, APIResponse>
    {
        private readonly IFeedbackRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFeedbackHandler(IFeedbackRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<APIResponse> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existFeedback = await _repository.GetById(request.FeedbackId);
                if (existFeedback == null)
                {
                    return new APIResponse
                    {
                        StatusResponse = System.Net.HttpStatusCode.NotFound,
                        Message = MessageCommon.NotFound,
                        Data = $"For feedback with id: {request.FeedbackId}"
                    };
                }
                await _repository.Delete(request.FeedbackId);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new APIResponse
                    {
                        StatusResponse = System.Net.HttpStatusCode.OK,
                        Message = MessageCommon.DeleteSuccessfully,
                        Data = $"For feedback with id: {request.FeedbackId}"
                    };
                }
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.OK,
                    Message = MessageCommon.DeleteSuccessfully,
                    Data = $"For feedback with id: {request.FeedbackId}"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageCommon.ServerError,
                    Data = ex.Message
                };
            }
        }
    }
}
