using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Repository;

public class ChatMessageRepository : RepositoryBase<ChatMessage>, IChatMessageRepository
{
    private readonly SaleProductDbContext _context;
    public ChatMessageRepository(SaleProductDbContext context) : base(context)
    {
        _context = context;
    }
}
