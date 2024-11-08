﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MobileProgramming.Business.Models.DTO.Order
{
    public record OrderDto(
        int OrderId,
        int? CartId,
        int? UserId,
        string PaymentMethod,
        string BillingAddress,
        string OrderStatus,
        string OrderUrl,
        DateTime OrderDate,
        CartOrderDto? Cart
    );
}
