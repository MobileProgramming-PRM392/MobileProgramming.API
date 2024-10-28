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

namespace MobileProgramming.Business.UseCase.Categories.Command.Create;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, APIResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category newCategory = new Category();
        newCategory.CategoryName = request.Category;
        await _categoryRepository.Add(newCategory);
        if(await _unitOfWork.SaveChangesAsync(cancellationToken) > 0)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.CreateSuccesfully,
                Data = request.Category
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.CreateFailed,
            Data = request.Category
        };
    }
}
