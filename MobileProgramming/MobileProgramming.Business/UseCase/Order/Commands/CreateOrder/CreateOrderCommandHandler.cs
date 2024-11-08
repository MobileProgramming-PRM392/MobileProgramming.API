
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MobileProgramming.Business.Hubs;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.UseCase.Notification.Commands.CreateNotification;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net;

namespace MobileProgramming.Business.UseCase.Order.Commands.CreateOrder;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, APIResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IZaloPayService _zaloPayService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
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
        IHubContext<NotificationHub, INotificationClient> hubContext,
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
        if (existUser == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = "User not found"
            };
        }

        if (request.CartItems == null || !request.CartItems.Any())
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = "Products are required"
            };
        }

        var productIds = request.CartItems.Select(ci => ci.ProductId).ToList();
        var quantities = request.CartItems.Select(ci => ci.Quantity).ToList();

        // Kiểm tra cartId dựa trên userId và các sản phẩm
        var cartId = await _cartItemRepository.GetCartIdByUserIdAndProducts(request.UserId, productIds, quantities);

        if (cartId == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = "Cart not found or cart items don't match."
            };
        }

        Random rnd = new Random();
        string app_trans_id = DateTime.UtcNow.ToString("yyMMdd") + "_" + rnd.Next(1000000);
        string cacheKey = $"payment_pending_{app_trans_id}";

        var result = await _zaloPayService.CreateOrderAsync(request.Amount, request.Description!, app_trans_id);
        var zp_trans_token = (string)result["zp_trans_token"];
        var orderId = await _orderRepository.CountOrdersAsync() + 1;
        var order = new Data.Entities.Order
        {
            CartId = cartId.Value, // Sử dụng cartId lấy được
            UserId = request.UserId,
            BillingAddress = request.BillingAddress ?? string.Empty,
            PaymentMethod = "Credit Card",
            OrderStatus = "Pending",
            OrderDate = DateTime.Now
        };

        await _orderRepository.Add(order);
        await _unitOfWork.SaveChangesAsync();

        // Tạo payment cho order
        var payment = new Data.Entities.Payment
        {
            PaymentId = app_trans_id,
            OrderId = order.OrderId,
            Amount = decimal.Parse(request.Amount),
            PaymentDate = DateTime.Now,
            PaymentStatus = "Active"
        };

        await _paymentRepository.Add(payment);
        await _unitOfWork.SaveChangesAsync();

        var hashEntries = new HashEntry[]
                            {
                            new HashEntry("transactionId",$"{app_trans_id}"),
                            };
        await _caching.HashSetAsync(cacheKey, hashEntries, 120);


        var notificationResponse = await _mediator.Send(new CreateNotification
        {
            UserId = request.UserId,
            Message = "Your order has been created successfully."
        });

        // Push notification via SignalR
        await _hubContext.Clients.User(request.UserId.ToString())
            .ReceiveNotification(JsonConvert.SerializeObject(new { message = "Order placed successfully" }));

        return new APIResponse
        {
            StatusResponse = HttpStatusCode.Created,
            Message = "Order created successfully",
            Data = result
        };
    }
}
