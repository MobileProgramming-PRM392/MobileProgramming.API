using MediatR;
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
        public List<int> ProductId { get; set; }
        public List<int> Quantity { get; set; }

        public AddCartItemCommand(int userId, List<int> productId, List<int> quantity)
        {
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
