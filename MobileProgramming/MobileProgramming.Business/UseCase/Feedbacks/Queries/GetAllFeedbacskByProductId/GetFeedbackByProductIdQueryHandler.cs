using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO;
using MobileProgramming.Business.Models.DTO.Feedbacks;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Queries.GetAllFeedbacksByProductId
{
    public class GetFeedbackByProductIdQueryHandler : IRequestHandler<GetFeedbackByProductIdQuery, APIResponse>
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;

        public GetFeedbackByProductIdQueryHandler(IFeedbackRepository feedbackRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetFeedbackByProductIdQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var feedbacks = await _feedbackRepository.GetFeedbackByProductId(request.ProductId);

            var result = _mapper.Map<List<FeedbackDtoResponse>>(feedbacks);

            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = result;

            return response;
        }
    }
}
