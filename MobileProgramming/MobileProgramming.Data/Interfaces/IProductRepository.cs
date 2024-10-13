using Microsoft.Extensions.Logging;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces.Common;
using MobileProgramming.Data.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetFilteredProductsAsync(ProductFilterDto filter, ProductSortDto sort);
        Task<List<Product>> GetProductsToDisplay();
        Task<Product?> GetProductDetail(int productId);
    }
}
