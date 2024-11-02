using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase.Order.Queries.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, APIResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<APIResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.FilterOrders(
                orderId: request.OrderId,
                userId: request.UserId,
                billingAddress: request.BillingAddress,
                status: request.OrderStatus,
                startDate: request.StartDate,
                endDate: request.EndDate
            );

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Orders retrieved successfully",
                Data = orders
            };
        }
    }
}
