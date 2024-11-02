namespace MobileProgramming.Business.Models.DTO.Order
{
    public class CartOrderItemDto
    {
        public int CartItemId { get; set; }

        public int? ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
