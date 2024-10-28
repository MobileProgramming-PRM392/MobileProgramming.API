using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Products.Command.DeleteProduct;

public class DeleteProductCommand: IRequest<APIResponse>
{
    public int ProductId { get; set; }

    public DeleteProductCommand(int productId)
    {
        ProductId = productId;
    }
}
