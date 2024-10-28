using MediatR;
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

namespace MobileProgramming.Business.UseCase.Products.Command.DeleteProductImage;

public class DeleteProductImageHandler : IRequestHandler<DeleteProductImageCommand, APIResponse>
{
    private readonly IImageService _imageService;
    private readonly IProductImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductImageHandler(IImageService imageService, IProductImageRepository imageRepository,
        IUnitOfWork unitOfWork)
    {
        _imageService = imageService;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ProductImage? image = await _imageRepository.GetByImageUrl(request.ImageUrl);
            if (image == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = null
                };
            }
            string url = image.ImageUrl!;
            int startIndex = url.LastIndexOf("/eventcontainer/") + "/eventcontainer/".Length;
            string result = url.Substring(startIndex);
            await _imageService.DeleteBlob(result);
            await _imageRepository.Delete(image.ImageId);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.DeleteSuccessfully,
                    Data = $"For image with url: {request.ImageUrl}"
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.DeleteFailed,
                Data = $"For image with url: {request.ImageUrl}"
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.ServerError,
                Data = ex.Message
            };
        }
    }
}
