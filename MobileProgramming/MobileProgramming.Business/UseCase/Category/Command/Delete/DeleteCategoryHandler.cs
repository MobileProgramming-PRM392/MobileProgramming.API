using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces.Common;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Entities;
using MobileProgramming.Business.Models.ResponseMessage;
using System.Net;

namespace MobileProgramming.Business.UseCase.Categories.Command.Delete;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, APIResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, 
        IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<APIResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? existCategory = await _categoryRepository.GetById(request.CategoryId);
        if (existCategory == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = request.CategoryId
            };
        }
        List<Product> affectedProduct = await _productRepository.GetbyCategoryId(request.CategoryId);
        foreach (Product product in affectedProduct)
        {
            product.CategoryId = null;
            await _productRepository.Update(product);
        }
        await _categoryRepository.Delete(request.CategoryId);
        if(await _unitOfWork.SaveChangesAsync(cancellationToken) > 0)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.DeleteSuccessfully,
                Data = null
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.DeleteFailed,
            Data = null
        };
    }
}
