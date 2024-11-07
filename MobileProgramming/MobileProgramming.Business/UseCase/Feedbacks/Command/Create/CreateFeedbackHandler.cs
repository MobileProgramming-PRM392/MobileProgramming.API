using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Feedbacks;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Command.Create
{
    public class CreateFeedbackHandler : IRequestHandler<CreateFeedbackCommand, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateFeedbackHandler(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newFeedback = new Feedback
                {
                    Comment = request.Dto.Comment,
                    CreatedAt = DateTime.Now,
                    ProductId = request.Dto.ProductId,
                    Rating = request.Dto.Rating,
                    UserId = request.Dto.UserId,
                };

                await _feedbackRepository.Add(newFeedback);
                if (await _unitOfWork.SaveChangesAsync(cancellationToken) > 0)
                {
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.OK,
                        Message = MessageCommon.CreateSuccesfully,
                        Data = _mapper.Map<FeedbackDtoResponse>(newFeedback)
                    };
                }
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.CreateFailed,
                    Data = _mapper.Map<FeedbackDtoResponse>(newFeedback)
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.ServerError,
                    Data = ex.Message
                };
            }
        }
    }
}
