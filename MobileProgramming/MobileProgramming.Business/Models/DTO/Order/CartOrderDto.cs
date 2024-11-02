using MobileProgramming.Business.Models.DTO.CartItems;
using System.Text.Json.Serialization;

namespace MobileProgramming.Business.Models.DTO.Order
{
    public class CartOrderDto
    {
        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = null!;
        //[JsonIgnore]
        public virtual ICollection<CartOrderItemDto> CartItems { get; set; } = new List<CartOrderItemDto>();
    }
}
