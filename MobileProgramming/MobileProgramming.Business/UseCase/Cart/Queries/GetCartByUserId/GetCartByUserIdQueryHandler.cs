using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase
{ 
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, APIResponse>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartByUserIdQueryHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<APIResponse> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);

            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = cart;

            return response;
        }
    }
}
