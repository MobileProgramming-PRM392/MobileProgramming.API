using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MobileProgramming.Business.Models.DTO.Order
{
    public record OrderDto(
        int OrderId,
        int? CartId,
        int? UserId,
        string PaymentMethod,
        string BillingAddress,
        string OrderStatus,
        DateTime OrderDate,
        CartOrderDto? Cart
    );
}
