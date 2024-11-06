using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Order.Queries.GetOrder
{
    public class GetOrderQuery : IRequest<APIResponse>
    {
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public string? BillingAddress { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //public GetOrderQuery(int? orderId = null, int? userId = null, string? billingAddress = null, string? orderStatus = null, DateTime? startDate = null, DateTime? endDate = null)
        //{
        //    OrderId = orderId;
        //    UserId = userId;
        //    BillingAddress = billingAddress;
        //    OrderStatus = orderStatus;
        //    StartDate = startDate;
        //    EndDate = endDate;
        //}
    }
}
