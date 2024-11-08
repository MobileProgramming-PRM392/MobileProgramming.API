﻿using Microsoft.EntityFrameworkCore;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Persistence;

namespace MobileProgramming.Data.Repository
{
    internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly SaleProductDbContext _context;

        public OrderRepository(SaleProductDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Payments)
                .Include(o => o.Cart)
                    .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .AsSplitQuery()
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }


        public async Task<int> CountOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<IEnumerable<Order>> FilterOrders(
            int? orderId = null,
            int? userId = null,
            string? billingAddress = null,
            string? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _context.Orders.Include(c => c.Cart).ThenInclude(c => c.CartItems).ThenInclude(c => c.Product).AsQueryable();

            if (orderId.HasValue)
            {
                query = query.Where(o => o.OrderId == orderId);
            }

            if (userId.HasValue)
            {
                query = query.Where(o => o.UserId == userId);
            }

            if (!string.IsNullOrEmpty(billingAddress))
            {
                query = query.Where(o => o.BillingAddress.Contains(billingAddress));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.OrderStatus == status);
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= endDate);
            }

            return await query.ToListAsync();
        }
    }
}
