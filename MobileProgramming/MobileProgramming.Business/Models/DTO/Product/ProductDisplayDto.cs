﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Product
{
    public class ProductDisplayDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;
        public string? ProductBrand { get; set; }

        public string? BriefDescription { get; set; }
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
    }
}
