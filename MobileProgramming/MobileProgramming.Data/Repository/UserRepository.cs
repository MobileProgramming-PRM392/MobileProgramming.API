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

namespace MobileProgramming.Data.Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly SaleProductDbContext _context;
    public UserRepository(SaleProductDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
    }
}
