namespace MobileProgramming.Business.Models.DTO.Order
{
    public class CartOrderItemDto
    {
        public int CartItemId { get; set; }

        public int? ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }

    }
}
