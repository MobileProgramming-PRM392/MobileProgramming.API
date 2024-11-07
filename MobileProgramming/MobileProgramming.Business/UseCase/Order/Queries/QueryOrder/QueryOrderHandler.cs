using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;

namespace MobileProgramming.Business.UseCase.Order.Queries.QueryOrder
{
    public class QueryOrderHandler : IRequestHandler<QueryOrder, APIResponse>
    {
        private readonly IZaloPayService _zaloPayService;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCaching _caching;

        public QueryOrderHandler(IZaloPayService zaloPayService, IOrderRepository orderRepository, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, IRedisCaching caching)
        {
            _zaloPayService = zaloPayService;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(QueryOrder request, CancellationToken cancellationToken)
        {
            var transactionId = await _caching.HashGetSpecificKeyAsync($"payment_{request.zp_trans_token}", "transactionId");
            var result = await _zaloPayService.QueryOrderStatus(transactionId);
            var returncode = Convert.ToInt32(result["return_code"]);
            var paymentexist = await _paymentRepository.GetById(request.zp_trans_token!);
            var exist = await _orderRepository.GetByIdAsync((int)paymentexist.OrderId);
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
                Message = "Successfully",
                Data = result,
            };



        }
    }
}
