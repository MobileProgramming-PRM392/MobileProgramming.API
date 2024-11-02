using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Order.Queries.QueryOrder
{
    public class QueryOrder : IRequest<APIResponse>
    {
        public string order_id { get; set; }

        public QueryOrder(string order_id)
        {
            this.order_id = order_id;
        }
    }
}
