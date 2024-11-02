using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<APIResponse>
    {
        public int? CartId { get; set; }

        public int? UserId { get; set; }

        public string? BillingAddress { get; set; }

        public string? Amount { get; set; }
        public string? Description { get; set; }

        public CreateOrderCommand(int? cartId, int? userId, string? billingAddress, string? amount, string? description)
        {
            CartId = cartId;
            UserId = userId;
            BillingAddress = billingAddress;
            Amount = amount;
            Description = description;  
        }
    }
}
