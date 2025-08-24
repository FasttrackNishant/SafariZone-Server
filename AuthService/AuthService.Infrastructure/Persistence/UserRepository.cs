using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence;

public class UserRepository(AuthDbContext _db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email)
    {
        return _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetOrCreateAadUserAsync(string aadId, string email, List<string> roles)
    {
        var roleId = MapRoleToInternalId(roles);
        var user = await _db.Users.FirstOrDefaultAsync(u => u.AadId == aadId);

        if (user == null)
        {
            user = new User
            {
                AadId = aadId,
                Email = email,
                RoleId = roleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }
        else
        {
            user.Email = email;
            user.RoleId = roleId;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        return user;
    }

    public int MapRoleToInternalId(List<string> roles)
    {
        var roleName = roles.First();
        var role = _db.Roles.FirstOrDefault(r => r.RoleName == roleName);
        if (role == null)
            throw new Exception($"Role {roleName} not found in DB");
        return role.RoleId;
    }
}