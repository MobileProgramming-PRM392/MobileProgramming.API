using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces.Common;
using MobileProgramming.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileProgramming.Data.Entities;
using MobileProgramming.Business.Models.ResponseMessage;
using System.Net;
using MobileProgramming.Business.Models.DTO.Product;

namespace MobileProgramming.Business.UseCase.Products.Command.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, 
        IImageService imageService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _imageService = imageService;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product? existProduct = await _productRepository.GetById(request.ProductId);
        if(existProduct == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            };
        }
        existProduct.Price = request.Dto.Price;
        existProduct.BriefDescription = request.Dto.BriefDescription;
        existProduct.FullDescription = request.Dto.FullDescription;
        existProduct.ProductName = request.Dto.ProductName;
        existProduct.ProductBrand = request.Dto.ProductBrand;
        existProduct.CategoryId = request.Dto.CategoryId;
        existProduct.TechnicalSpecifications = request.Dto.TechnicalSpecifications;
        try
        {
            await ProcessImages(request.Dto.Images, existProduct);

            await _productRepository.Update(existProduct);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.UpdateSuccesfully,
                    Data = _mapper.Map<ProductDisplayDto>(existProduct)
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.UpdateFailed,
                Data = null
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


    private async Task ProcessImages(IEnumerable<ProductImageDto> images, Product newProduct)
    {
        if (images == null || !images.Any()) return;

        foreach (var image in images)
        {
            var newImage = new ProductImage
            {
                ProductId = newProduct.ProductId,
                ImageUrl = await _imageService.UploadImage(image.base64!, $"{newProduct.ProductName}-{Guid.NewGuid()}")
            };

            if (!string.IsNullOrEmpty(newImage.ImageUrl))
            {
                newProduct.ProductImages.Add(newImage);
            }
        }
    }
}
