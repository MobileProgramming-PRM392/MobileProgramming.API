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

        public async Task<int?> GetCartIdByUserIdAndProducts(int userId, List<int> productIds, List<int> quantities)
        {
            // Lấy giỏ hàng của người dùng
            var cart = await _context.Carts
                                     .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null)
            {
                // Nếu không tìm thấy giỏ hàng, trả về null
                return null;
            }

            // Lặp qua các productId và quantity để kiểm tra
            for (int i = 0; i < productIds.Count; i++)
            {
                var existingCartItem = await _context.CartItems
                                                     .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId
                                                                                && ci.ProductId == productIds[i]);

                if (existingCartItem == null || existingCartItem.Quantity != quantities[i])
                {
                    // Nếu không có hoặc quantity không khớp, thì trả về null
                    return null;
                }
            }

            // Trả về cartId nếu tất cả các điều kiện thỏa mãn
            return cart.CartId;
        }
        
    }
}
