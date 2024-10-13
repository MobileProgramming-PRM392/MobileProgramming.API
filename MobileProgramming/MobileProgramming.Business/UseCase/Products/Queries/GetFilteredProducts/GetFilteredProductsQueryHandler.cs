using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Business.UseCase.Products.Queries.GetAllProducts;
using MobileProgramming.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Products.Queries.GetFilteredProducts
{
    public class GetFilteredProductsQueryHandler : IRequestHandler<GetFilteredProductsQuery, APIResponse>
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public GetFilteredProductsQueryHandler(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();

            var result = await _productRepo.GetFilteredProductsAsync(request.Filter, request.Sort);

            var listProduct = _mapper.Map<IEnumerable<ProductDisplayDto>>(result);
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = listProduct;
            return response;
        }
    }
}
