using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Product;

public class CreateProductDto
{
    public string ProductName { get; set; } = null!;

    public string? ProductBrand { get; set; }

    public string? BriefDescription { get; set; }

    public string? FullDescription { get; set; }

    public string? TechnicalSpecifications { get; set; }

    public decimal Price { get; set; }

    public int? CategoryId { get; set; }
    public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
}
