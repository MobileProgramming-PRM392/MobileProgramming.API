using MediatR;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Products.Queries.GetFilteredProducts
{
    public class GetFilteredProductsQuery : IRequest<APIResponse>
    {
        public ProductFilterDto Filter { get; }
        public ProductSortDto Sort { get; }

        public GetFilteredProductsQuery(ProductFilterDto filter, ProductSortDto sort)
        {
            Filter = filter;
            Sort = sort;
        }
    }
}
