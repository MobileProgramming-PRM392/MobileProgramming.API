using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces.Common;

namespace MobileProgramming.Data.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<List<Notification>> GetNotificationsByFilter(int userId, bool? isRead);
    }
}
