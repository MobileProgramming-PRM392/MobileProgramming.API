using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Order;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase.Order.Queries.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, APIResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
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

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Orders retrieved successfully",
                Data = orderDtos
            };
        }
    }
}
