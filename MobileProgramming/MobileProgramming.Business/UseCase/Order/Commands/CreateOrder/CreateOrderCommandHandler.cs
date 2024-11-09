
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MobileProgramming.Business.Hubs;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.UseCase.Notification.Commands.CreateNotification;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using MobileProgramming.Data.Repository;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net;

namespace MobileProgramming.Business.UseCase.Order.Commands.CreateOrder;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, APIResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IZaloPayService _zaloPayService;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
    private readonly IRedisCaching _caching;
    private readonly IMediator _mediator; // Add MediatR mediator

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUserRepository userRepository, ICartItemRepository cartItemRepository, ICartRepository cartRepository, IPaymentRepository paymentRepository, IZaloPayService zaloPayService, IProductRepository productRepository, IUnitOfWork unitOfWork, IHubContext<NotificationHub, INotificationClient> hubContext, IRedisCaching caching, IMediator mediator)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _cartItemRepository = cartItemRepository;
        _cartRepository = cartRepository;
        _paymentRepository = paymentRepository;
        _zaloPayService = zaloPayService;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _caching = caching;
        _mediator = mediator;
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

       
        decimal totalPrice = 0;
        foreach (var cartItem in request.CartItems)
        {
            var product = await _productRepository.GetById(cartItem.ProductId);

            totalPrice += product.Price * cartItem.Quantity;

        }

        var newCart = new Cart
        {
            UserId = request.UserId,
            TotalPrice = totalPrice,
            Status = "unactive"
        };

        await _cartRepository.Add(newCart);
        await _unitOfWork.SaveChangesAsync();

        
        foreach (var cartItem in request.CartItems)
        {
            var product = await _productRepository.GetById(cartItem.ProductId);

            var newCartItem = new Data.Entities.CartItem
            {
                CartId = newCart.CartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = product.Price
            };

            await _cartItemRepository.Add(newCartItem);
        }

        await _unitOfWork.SaveChangesAsync(); 
        var cartId = newCart.CartId;


        Random rnd = new Random();
        string app_trans_id = DateTime.UtcNow.ToString("yyMMdd") + "_" + rnd.Next(1000000);
        string cacheKey = $"payment_pending_{app_trans_id}";

        var result = await _zaloPayService.CreateOrderAsync(request.Amount, request.Description!, app_trans_id);
        var zp_trans_token = (string)result["zp_trans_token"];
        var orderUrl = (string)result["order_url"];
        var orderId = await _orderRepository.CountOrdersAsync() + 1;
        var order = new Data.Entities.Order
        {
            CartId = cartId, 
            UserId = request.UserId,
            BillingAddress = request.BillingAddress ?? string.Empty,
            PaymentMethod = "Credit Card",
            OrderStatus = "Pending",
            OrderDate = DateTime.Now,
            OrderUrl = orderUrl,
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
