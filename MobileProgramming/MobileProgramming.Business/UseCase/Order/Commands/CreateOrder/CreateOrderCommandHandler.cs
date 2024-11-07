using MediatR;
using Microsoft.AspNetCore.SignalR;
using MobileProgramming.Business.Hub;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.UseCase.Notification.Commands.CreateNotification;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net;

namespace MobileProgramming.Business.UseCase.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, APIResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IZaloPayService _zaloPayService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IRedisCaching _caching;
        private readonly IMediator _mediator; // Add MediatR mediator

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ICartItemRepository cartItemRepository,
            IZaloPayService zaloPayService,
            IUnitOfWork unitOfWork,
            IPaymentRepository paymentRepository,
            IRedisCaching caching,
            IHubContext<NotificationHub> hubContext,
            IMediator mediator) // Inject MediatR mediator
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _cartItemRepository = cartItemRepository;
            _zaloPayService = zaloPayService;
            _unitOfWork = unitOfWork;
            _caching = caching;
            _hubContext = hubContext;
            _mediator = mediator;
            _paymentRepository = paymentRepository;
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

            var result = await _zaloPayService.CreateOrderAsync(request.Amount, request.Description!, app_trans_id);
            var zp_trans_token = (string)result["zp_trans_token"];
            var orderId = await _orderRepository.CountOrdersAsync() + 1;
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

            var payment = new Data.Entities.Payment
            {
                PaymentId = zp_trans_token,
                OrderId = order.OrderId, 
                Amount = decimal.Parse(request.Amount),
                PaymentDate = DateTime.Now,
                PaymentStatus = "Active"
            };

            await _paymentRepository.Add(payment);
            await _unitOfWork.SaveChangesAsync();

            var notification = new Data.Entities.Notification
            {
                UserId = order.UserId,
                Message = "Your order has been placed successfully!",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            var notificationResponse = await _mediator.Send(new CreateNotification
            {
                UserId = (int)order.UserId,
                Message = "Your order has been created successfully."
            });


            await _hubContext.Clients.User(request.UserId.ToString())
                .SendAsync("ReceiveNotification", JsonConvert.SerializeObject(notification, Formatting.Indented));

            var hashEntries = new HashEntry[]
                           {
                            new HashEntry("transactionId",$"{app_trans_id}")
                           };
            await _caching.HashSetAsync($"payment_{zp_trans_token}", hashEntries, 1000020);

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.Created,
                Message = "Order created successfully.",
                Data = result
            };
        }
    }
}
