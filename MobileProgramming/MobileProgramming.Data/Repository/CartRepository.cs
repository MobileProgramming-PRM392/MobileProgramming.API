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
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        private readonly SaleProductDbContext _context;

        public CartRepository(SaleProductDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cart?> GetActiveCartByUserIdAsync(int userId)
        {
            return await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "active");
        }
    }
}
