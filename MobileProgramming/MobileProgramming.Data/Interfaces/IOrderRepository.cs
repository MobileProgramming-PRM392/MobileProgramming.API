using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces.Common;

namespace MobileProgramming.Data.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> FilterOrders(
            int? orderId = null,
            int? userId = null,
            string? billingAddress = null,
            string? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null);
    }
}
