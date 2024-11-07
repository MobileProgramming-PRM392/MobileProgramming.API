using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Order.Commands.CreateOrder
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderCommand : IRequest<APIResponse>
    {
        public List<CartItemDto> CartItems { get; set; }

        public int UserId { get; set; }

        public string? BillingAddress { get; set; }

        public string? Amount { get; set; }
        public string? Description { get; set; }

        public CreateOrderCommand(List<CartItemDto> cartItems, int userId, string? billingAddress, string? amount, string? description)
        {
            CartItems = cartItems;
            UserId = userId;
            BillingAddress = billingAddress;
            Amount = amount;
            Description = description;
        }
    }
}
