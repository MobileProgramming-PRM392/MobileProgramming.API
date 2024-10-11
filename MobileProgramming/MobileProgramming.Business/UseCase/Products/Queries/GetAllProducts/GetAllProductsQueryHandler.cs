using MediatR;
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

namespace MobileProgramming.Business.UseCase.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, APIResponse>
    {
        private readonly IProductRepository _productRepo;

        public GetAllProductsQueryHandler(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<APIResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var result = await _productRepo.GetAll();
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = result;
            return response;
        }
    }
}
