using MediatR;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using System.ComponentModel.DataAnnotations;


namespace MobileProgramming.Business.UseCase.Products.Command.CreateProduct;

public class CreateProductCommand: IRequest<APIResponse>
{
    public CreateProductDto Dto { get; set; }

    public CreateProductCommand(CreateProductDto dto)
    {
        Dto = dto;
    }
}
