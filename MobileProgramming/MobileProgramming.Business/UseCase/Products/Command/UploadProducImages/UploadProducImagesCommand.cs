using MediatR;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Products.Command.UploadProducImages;

public class UploadProducImagesCommand: IRequest<APIResponse>
{
    public int ProductId { get; set; }
    public List<ProductImageDto> ProductImage { get; set; }

    public UploadProducImagesCommand(int productId, List<ProductImageDto> productImage)
    {
        ProductId = productId;
        ProductImage = productImage;
    }
}
