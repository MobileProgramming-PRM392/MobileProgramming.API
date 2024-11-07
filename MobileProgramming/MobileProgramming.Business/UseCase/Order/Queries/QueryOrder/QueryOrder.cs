using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Order.Queries.QueryOrder
{
    public class QueryOrder : IRequest<APIResponse>
    {
        public string zp_trans_token { get; set; }

        public QueryOrder(string zp_trans_token)
        {
            this.zp_trans_token = zp_trans_token;
        }
    }
}
