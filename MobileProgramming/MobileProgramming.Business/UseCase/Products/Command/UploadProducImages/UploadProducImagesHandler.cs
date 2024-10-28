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
using System.Net;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Business.Models.DTO.Product;

namespace MobileProgramming.Business.UseCase.Products.Command.UploadProducImages;

public class UploadProducImagesHandler : IRequestHandler<UploadProducImagesCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IImageService _imageService;

    public UploadProducImagesHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _imageService = imageService;
    }

    public async Task<APIResponse> Handle(UploadProducImagesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Product? producExist = await _productRepository.GetById(request.ProductId);
            if (producExist == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = null
                };
            }
            foreach (var image in request.ProductImage)
            {
                var newImage = new ProductImage
                {
                    ProductId = producExist.ProductId,
                    ImageUrl = await _imageService.UploadImage(image.base64!, $"{producExist.ProductName}-{Guid.NewGuid()}")
                };

                if (!string.IsNullOrEmpty(newImage.ImageUrl))
                {
                    producExist.ProductImages.Add(newImage);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.CreateSuccesfully,
                Data = null
            };
        }catch (Exception ex)
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
