using Microsoft.EntityFrameworkCore;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Persistence;

namespace MobileProgramming.Data.Repository
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        private readonly SaleProductDbContext _context;

        public PaymentRepository(SaleProductDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> FilterPaymentsAsync(
            string? paymentId,
            int? orderId)
        {
            var query = _context.Payments
                .Include(c => c.Order)
                .ThenInclude(c => c.Cart)
                .ThenInclude(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(paymentId))
            {
                query = query.Where(p => p.PaymentId == paymentId);
            }
            if (orderId.HasValue)
            {
                query = query.Where(p => p.OrderId == orderId);
            }


            // Execute the query and return results
            return await query.ToListAsync();
        }
    }
}
