using MediatR;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;


namespace MobileProgramming.Business.UseCase.Products.Command.CreateProduct;

public class CreateProductCommand: IRequest<APIResponse>
{
    public CreateProductDto Dto { get; set; }

    public CreateProductCommand(CreateProductDto dto)
    {
        Dto = dto;
    }
}
