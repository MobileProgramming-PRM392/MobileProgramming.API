using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Category;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase.Category.Queries.GetAllCategory
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoryQueryHandler(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<APIResponse> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var result = await _categoryRepository.GetAll();

            var listCategory = _mapper.Map<IEnumerable<CategoryDto>>(result);
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = listCategory;

            return response;
        }
    }
}
