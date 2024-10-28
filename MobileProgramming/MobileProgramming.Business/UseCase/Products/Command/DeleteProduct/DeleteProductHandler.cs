using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;

namespace MobileProgramming.Business.UseCase.Products.Command.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, APIResponse>
{
    private readonly IProductRepository _repository;
    private readonly IProductImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public DeleteProductHandler(IProductRepository repository, IProductImageRepository imageRepository, 
        IUnitOfWork unitOfWork, IImageService imageService)
    {
        _repository = repository;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public async Task<APIResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Product? existProduct = await _repository.GetById(request.ProductId);
            if (existProduct == null)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = $"For product with id: {request.ProductId}"
                };
            }
            List<ProductImage> images = await _imageRepository.GetByProductId(request.ProductId);
            if (images.Any())
            {
                foreach (ProductImage image in images)
                {
                    string url = image.ImageUrl!;
                    int startIndex = url.LastIndexOf("/eventcontainer/") + "/eventcontainer/".Length;
                    string result = url.Substring(startIndex);
                    await _imageService.DeleteBlob(result);
                    await _imageRepository.Delete(image.ImageId);
                }
                await _repository.Delete(request.ProductId);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new APIResponse
                    {
                        StatusResponse = System.Net.HttpStatusCode.OK,
                        Message = MessageCommon.DeleteSuccessfully,
                        Data = $"For product with id: {request.ProductId}"
                    };
                }
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = $"For product with id: {request.ProductId}"
                };
            }
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.DeleteSuccessfully,
                Data = $"For product with id: {request.ProductId}"
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.BadRequest,
                Message = MessageCommon.ServerError,
                Data =ex.Message
            };
        }
    }
}
