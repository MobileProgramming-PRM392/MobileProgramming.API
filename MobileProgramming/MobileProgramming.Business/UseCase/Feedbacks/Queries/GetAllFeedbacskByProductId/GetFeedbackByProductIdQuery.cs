using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Queries.GetAllFeedbacksByProductId
{
    public class GetFeedbackByProductIdQuery : IRequest<APIResponse>
    {
        public int ProductId { get; set; }

        public GetFeedbackByProductIdQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
