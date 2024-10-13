using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase.Products.Queries.GetProductDetail
{
    public class GetProductDetailQueryHandler : IRequestHandler<GetProductDetailQuery, APIResponse>
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly IProductImageRepository _productImageRepo;

        public GetProductDetailQueryHandler(IProductRepository productRepo, IMapper mapper, IProductImageRepository productImageRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _productImageRepo = productImageRepo;
        }

        public async Task<APIResponse> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var result = await _productRepo.GetProductDetail(request.ProductId);
            if(result == null)
            {
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageCommon.NotFound;
                response.Data = null;
            } else
            {
                var product = _mapper.Map<ProductDetailDto>(result);
                product.ImageUrl = await _productImageRepo.GetImageUrlByProductId(request.ProductId);
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.GetSuccesfully;
                response.Data = product;
            }

            return response;
        }
    }
}
