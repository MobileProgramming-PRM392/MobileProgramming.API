using Microsoft.EntityFrameworkCore;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Repository
{
    public class ProductImageRepository : RepositoryBase<ProductImage>, IProductImageRepository
    {
        private readonly SaleProductDbContext _context;

        public ProductImageRepository(SaleProductDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetImageUrlByProductId(int productId)
        {
            return await _context.ProductImages.Where(pi => pi.ProductId == productId).Select(pi => pi.ImageUrl).ToListAsync();
        }
    }
}
