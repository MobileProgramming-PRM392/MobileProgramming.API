using MediatR;
using MobileProgramming.Business.Models.DTO.CartItems;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase
{
    public class AddCartItemCommand : IRequest<APIResponse>
    {
        public int UserId { get; set; }
        public List<AddCartItemRequestDto> CartItems { get; set; }

        public AddCartItemCommand(int userId, List<AddCartItemRequestDto> cartItems)
        {
            UserId = userId;
            CartItems = cartItems;
        }
    }

}
