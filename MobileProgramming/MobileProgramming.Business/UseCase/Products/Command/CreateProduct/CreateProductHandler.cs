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
            foreach(var image in request.Dto.Images)
            {
                Guid id = Guid.NewGuid();
                ProductImage newImage = new ProductImage();
                newImage.ProductId = newProduct.ProductId;
                string? imageUrl = await _imageService.UploadImage(image.base64!, newProduct.ProductName + "-" + id.ToString());
                if (imageUrl != null)
                {
                    newImage.ImageUrl = imageUrl;
                }
                newProduct.ProductImages.Add(newImage);
            }
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
}
