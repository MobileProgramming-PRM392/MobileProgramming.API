using MediatR;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Products.Command.UpdateProduct;

public class UpdateProductCommand : IRequest<APIResponse>
{
    public int ProductId { get; set; }
    public CreateProductDto Dto { get; set; }

    public UpdateProductCommand(int productId, CreateProductDto dto)
    {
        ProductId = productId;
        Dto = dto;
    }
}
