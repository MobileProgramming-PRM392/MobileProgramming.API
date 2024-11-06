using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MobileProgramming.Data.Repository
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        private readonly SaleProductDbContext _context;

        public NotificationRepository(SaleProductDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetNotificationsByFilter(int userId, bool? isRead)
        {
            var query = _context.Notifications.AsQueryable();

            query = query.Where(n => n.UserId == userId);

            if (isRead.HasValue)
            {
                query = query.Where(n => n.IsRead == isRead.Value);
            }

            return await query.ToListAsync();
        }
    }
}
