using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.CartItems
{
    public class AddCartItemResponseDto
    {
        public int UserId { get; set; }
        public List<AddCartItemRequestDto> CartItems { get; set; }
        public int CartId { get; set; }
    }
}
