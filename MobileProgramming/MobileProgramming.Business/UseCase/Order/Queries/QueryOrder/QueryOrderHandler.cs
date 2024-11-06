using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;

namespace MobileProgramming.Business.UseCase.Order.Queries.QueryOrder
{
    public class QueryOrderHandler : IRequestHandler<QueryOrder, APIResponse>
    {
        private readonly IZaloPayService _zaloPayService;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QueryOrderHandler(IZaloPayService zaloPayService, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _zaloPayService = zaloPayService;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(QueryOrder request, CancellationToken cancellationToken)
        {
            var result = await _zaloPayService.QueryOrderStatus(request.order_id!);
            var returncode = Convert.ToInt32(result["return_code"]);
            var exist = await _orderRepository.GetById(request.order_id!);
            if (exist == null)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.NotFound,
                    Message = "Can't find this transaction",
                    Data = null,
                };
            }

            //switch expression
            exist.OrderStatus = returncode switch
            {
                1 => "Success",
                2 => "Fail",
                _ => exist.OrderStatus
            };

            await _orderRepository.Update(exist);
            await _unitOfWork.SaveChangesAsync();

            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.NotFound,
                Message = "Can't find this transaction",
                Data = result,
            };



        }
    }
}
