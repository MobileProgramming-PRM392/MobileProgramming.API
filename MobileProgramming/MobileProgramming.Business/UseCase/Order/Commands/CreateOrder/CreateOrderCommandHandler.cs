using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.ExternalServices.Payment.ZaloPay;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, APIResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IZaloPayService _zaloPayService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUserRepository userRepository, ICartItemRepository cartItemRepository, IZaloPayService zaloPayService, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _cartItemRepository = cartItemRepository;
            _zaloPayService = zaloPayService;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetById(request.UserId);
            var existCart = await _cartItemRepository.GetById(request.CartId);
            if (existUser == null || existCart == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.Created,
                    Message = "User or Cart not existed"
                };
            }

            Random rnd = new Random();
            string app_trans_id = DateTime.UtcNow.ToString("yyMMdd") + "_" + rnd.Next(1000000);
            string cacheKey = $"payment_{app_trans_id}";
            var appUser = "user123";

            //call api to create transaction
            var result = await _zaloPayService.CreateOrderAsync(request.Amount, appUser, request.Description!, app_trans_id);
            var return_code = (long)result["return_code"];
            var return_message = (string)result["return_message"];
            var zp_trans_token = (string)result["zp_trans_token"];
            var order_token = (string)result["order_token"];
            var qr_code = (string)result["qr_code"];

            var order = new Data.Entities.Order
            {
                CartId = request.CartId,
                UserId = request.UserId,
                BillingAddress = request.BillingAddress ?? string.Empty,
                PaymentMethod = "Credit Card",
                OrderStatus = "Pending",
                OrderDate = DateTime.Now
            };

            await _orderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.Created,
                Message = "Order created successfully.",
                Data = new
                {
                    return_code = return_code,
                    returnMessage = return_message,
                    zpTransToken = zp_trans_token,
                    orderToken = order_token,
                    qrCode = qr_code
                }
            };

        }
    }
}
