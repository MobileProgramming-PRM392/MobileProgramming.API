using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Category;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System.Net;

namespace MobileProgramming.Business.UseCase.Category.Queries.GetAllCategory
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, APIResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRedisCaching _caching;

        public GetAllCategoryQueryHandler(IMapper mapper, ICategoryRepository categoryRepository, IRedisCaching caching)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();

            var cacheKey = "all_categories";
            var cachingData = await _caching.GetAsync<List<CategoryDto>>(cacheKey);
            if (cachingData != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = cachingData
                };
            }

            var result = await _categoryRepository.GetAll();

            var listCategory = _mapper.Map<IEnumerable<CategoryDto>>(result);
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.GetSuccesfully;
            response.Data = listCategory;
            await _caching.SetAsync(cacheKey, listCategory, 5);
            return response;
        }
    }
}
