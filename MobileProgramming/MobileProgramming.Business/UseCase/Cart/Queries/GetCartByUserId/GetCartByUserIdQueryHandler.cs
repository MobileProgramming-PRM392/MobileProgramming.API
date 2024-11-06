using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase
{
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, APIResponse>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartByUserIdQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);

            var result = _mapper.Map<CartDto>(cart);

            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = result;

            return response;
        }
    }
}
