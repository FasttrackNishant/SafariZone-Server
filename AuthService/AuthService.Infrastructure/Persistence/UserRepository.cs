using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence;

public class UserRepository(AuthDbContext db) : IUserRepository
{
    
    public Task<User?> GetByEmailAsync(string email)
    {
        return db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
    }
}