using MobileProgramming.Business.Models.DTO.CartItems;
using MobileProgramming.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO
{
    public class CartDto
    {
        public int CartId { get; set; }

        public int? UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
    }
}
