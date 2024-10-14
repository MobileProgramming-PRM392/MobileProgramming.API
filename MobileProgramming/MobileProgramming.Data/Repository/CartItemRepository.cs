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
    public class CartItemRepository : RepositoryBase<CartItem>, ICartItemRepository
    {
        private readonly SaleProductDbContext _context;

        public CartItemRepository(SaleProductDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
    
    }
}
