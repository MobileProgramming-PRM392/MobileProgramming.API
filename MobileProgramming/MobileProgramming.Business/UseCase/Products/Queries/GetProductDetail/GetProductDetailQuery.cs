using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Products.Queries.GetProductDetail
{
    public class GetProductDetailQuery : IRequest<APIResponse>
    {
        public int ProductId { get; set; }

        public GetProductDetailQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
