using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Feedbacks;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Business.UseCase.Feedbacks.Queries.GetAllFeedbacksByProductId;
using MobileProgramming.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Queries.GetMyFeedbackByProductId
{
    public class GetMyFeedbackByProductIdQueryHandler : IRequestHandler<GetMyFeedbackByProductIdQuery, APIResponse>
    {

        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;

        public GetMyFeedbackByProductIdQueryHandler(IFeedbackRepository feedbackRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }


        public async Task<APIResponse> Handle(GetMyFeedbackByProductIdQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var feedbacks = await _feedbackRepository.GetMyFeedbackByProductId(request.ProductId, request.UserId);

            var result = _mapper.Map<FeedbackDtoResponse>(feedbacks);

            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = result;

            return response;
        }
    }
}
