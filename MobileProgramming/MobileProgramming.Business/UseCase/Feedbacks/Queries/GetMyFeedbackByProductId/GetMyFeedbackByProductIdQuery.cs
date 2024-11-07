using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Queries.GetMyFeedbackByProductId
{
    public class GetMyFeedbackByProductIdQuery : IRequest<APIResponse>
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public GetMyFeedbackByProductIdQuery(int productId, int userId)
        {
            ProductId = productId;
            UserId = userId;
        }
    }
}
