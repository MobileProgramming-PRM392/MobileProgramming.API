using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces.Common;
using static MobileProgramming.Data.Repository.CartItemRepository;

namespace MobileProgramming.Data.Interfaces
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<int?> GetCartIdByUserIdAndProducts(int userId, List<int> productIds, List<int> quantities);
        Task DeleteCartItemByCartId(int cartId);
    }
}
