using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetActiveCartByUserIdAsync(int userId);
    }
}
