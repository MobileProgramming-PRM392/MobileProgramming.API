using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Feedbacks;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Command.Update
{
    public class UpdateFeebackHandler : IRequestHandler<UpdateFeedbackCommand, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFeedbackRepository _feedbackRepository;

        public UpdateFeebackHandler(IUnitOfWork unitOfWork, IFeedbackRepository feedbackRepository, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _feedbackRepository = feedbackRepository;
        }
        public async Task<APIResponse> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existFeedback = await _feedbackRepository.GetById(request.FeedbackId);
                if (existFeedback == null)
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.NotFound,
                        Message = MessageCommon.NotFound,
                        Data = null
                    };
                }

                existFeedback.Rating = request.Dto.Rating;
                existFeedback.Comment = request.Dto.Comment;

                await _feedbackRepository.Update(existFeedback);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    existFeedback.User = null;
                    existFeedback.Product = null;
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.OK,
                        Message = MessageCommon.Complete,
                        Data = _mapper.Map<FeedbackDtoResponse>(existFeedback)
                    };
                }
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.UpdateFailed,
                    Data = _mapper.Map<FeedbackDtoResponse>(existFeedback)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.UpdateFailed,
                    Data = ex.Message
                };
            }
        }
    }
}
