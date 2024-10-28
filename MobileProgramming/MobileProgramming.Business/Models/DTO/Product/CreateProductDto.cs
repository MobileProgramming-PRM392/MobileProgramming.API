using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Product;

public class CreateProductDto
{
    [Required(ErrorMessage ="ProductName is required!")]
    public string ProductName { get; set; } = null!;

    [Required(ErrorMessage = "ProductBrand is required!")]
    public string? ProductBrand { get; set; }

    [Required(ErrorMessage = "BriefDescription is required!")]
    public string? BriefDescription { get; set; }

    [Required(ErrorMessage = "FullDescription is required!")]
    public string? FullDescription { get; set; }

    public string? TechnicalSpecifications { get; set; }

    [Required(ErrorMessage = "Price is required!")]
    [Range(0.00, 1000_000_000.00, ErrorMessage ="Price ranged between 0 and 1000 000 000")]
    public decimal Price { get; set; }

    public int? CategoryId { get; set; }
    public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
}
