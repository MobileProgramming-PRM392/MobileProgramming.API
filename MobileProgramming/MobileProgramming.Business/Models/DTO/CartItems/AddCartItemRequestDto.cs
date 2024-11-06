using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.CartItems
{
    public class AddCartItemRequestDto
    {
        public List<int> ProductId { get; set; }
        public List<int> Quantity { get; set; }
    }
}
