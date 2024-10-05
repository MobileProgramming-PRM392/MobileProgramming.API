using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }       
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public decimal? Rating { get; set; } 
        public long CreatedAt { get; set; }
        public long UpdateAt { get; set; }
    }
}
