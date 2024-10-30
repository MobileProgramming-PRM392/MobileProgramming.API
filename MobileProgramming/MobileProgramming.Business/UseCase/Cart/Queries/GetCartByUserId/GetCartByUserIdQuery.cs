using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase
{
    public class GetCartByUserIdQuery : IRequest<APIResponse>
    {
        public int UserId { get; set; }


        public GetCartByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
