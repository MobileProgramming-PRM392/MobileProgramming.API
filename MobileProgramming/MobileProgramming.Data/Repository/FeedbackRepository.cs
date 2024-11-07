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
    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        private readonly SaleProductDbContext _context;

        public FeedbackRepository(SaleProductDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Feedback>> GetFeedbackByProductId(int productId)
        {
           return await _context.Feedbacks.Where(f => f.ProductId == productId).ToListAsync();
        }

        public async Task<Feedback> GetMyFeedbackByProductId(int productId, int userId)
        {
            return await _context.Feedbacks.Where(f => f.ProductId == productId && f.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
