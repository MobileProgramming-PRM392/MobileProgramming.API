namespace MobileProgramming.Business.Models.DTO.Order
{
    public record PaymentDto
    {
        public string PaymentId { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
        public int OrderId { get; set; }
        public int? CartId { get; set; }
        public int? UserId { get; set; }
        public string? OrderUrl { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string BillingAddress { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public CartOrderDto? Cart { get; set; }
    }
}
