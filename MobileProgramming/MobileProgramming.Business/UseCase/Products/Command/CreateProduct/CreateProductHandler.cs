using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Product;
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

namespace MobileProgramming.Business.UseCase.Products.Command.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductImageRepository _imageRepository;
    private readonly IProductRepository _productRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public CreateProductHandler(IUnitOfWork unitOfWork, IProductImageRepository imageRepository, 
        IProductRepository productRepository, IMapper mapper, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageRepository = imageRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<APIResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product newProduct = _mapper.Map<Product>(request.Dto);
        try
        {
            await ProcessImages(request.Dto.Images, newProduct);

            await _productRepository.Add(newProduct);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.CreateSuccesfully,
                    Data = _mapper.Map<ProductDisplayDto>(newProduct)
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.CreateFailed,
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.CreateFailed,
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
