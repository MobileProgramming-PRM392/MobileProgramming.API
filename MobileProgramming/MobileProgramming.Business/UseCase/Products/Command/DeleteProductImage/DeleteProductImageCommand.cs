using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Products.Command.DeleteProductImage;

public class DeleteProductImageCommand: IRequest<APIResponse>
{
    public string ImageUrl { get; set; }

    public DeleteProductImageCommand(string imageUrl)
    {
        ImageUrl = imageUrl;
    }
}
