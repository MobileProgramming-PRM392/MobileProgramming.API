using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Order.Queries.QueryOrder
{
    public class QueryOrder : IRequest<APIResponse>
    {
        public string transactionId { get; set; }

        public QueryOrder(string zp_trans_token)
        {
            this.transactionId = zp_trans_token;
        }
    }
}
