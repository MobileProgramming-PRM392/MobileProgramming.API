using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Product;

public class UploadImageDto
{
    public int? ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;
}
