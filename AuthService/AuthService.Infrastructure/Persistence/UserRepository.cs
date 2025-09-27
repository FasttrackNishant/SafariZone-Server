using AuthService.Core.DTO;
using AuthService.Core.Entities;
using AuthService.Core.Helpers;
using AuthService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Persistence;

public class UserRepository(AuthDbContext db, ILogger<UserRepository> logger) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email)
    {
        return db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetOrCreateAadUserAsync(string aadId, string email, List<string> roles)
    {
        var roleId = MapRoleToInternalId(roles);
        var user = await db.Users.FirstOrDefaultAsync(u => u.AadId == aadId);

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
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }
        else
        {
            user.Email = email;
            user.RoleId = roleId;
            user.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }

        return user;
    }

    public int MapRoleToInternalId(List<string> roles)
    {
        var roleName = roles.First();
        var role = db.Roles.FirstOrDefault(r => r.RoleName == roleName);
        if (role == null)
            throw new Exception($"Role {roleName} not found in DB");
        return role.RoleId;
    }

    public async Task<OperationResult<User>> SignupUser(User newUser)
    {
        try
        {
            // 1️⃣ Check if user already exists
            var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);

            if (existingUser != null)
            {
                if (!existingUser.IsActive)
                {
                    // Reactivate user
                    existingUser.IsActive = true;
                    existingUser.TermsVersion = newUser.TermsVersion;
                    existingUser.TermsAgreedOn = DateTime.UtcNow;

                    var (hash, salt) = PasswordHelper.HashPassword(newUser.PasswordHash);
                    existingUser.PasswordHash = hash;
                    existingUser.Salt = salt;

                    await db.SaveChangesAsync();

                    return new OperationResult<User>
                    {
                        Success = true,
                        Data = existingUser,
                    };
                }

                return new OperationResult<User>
                {
                    Success = false,
                    ErrorMessage = "User already exists",
                    Data = existingUser
                };
            }

            // 2️⃣ New user
            var (newHash, newSalt) = PasswordHelper.HashPassword(newUser.PasswordHash);
            newUser.PasswordHash = newHash;
            newUser.Salt = newSalt;
            newUser.IsActive = true;
            newUser.TermsAgreedOn = DateTime.UtcNow;

            await db.Users.AddAsync(newUser);
            await db.SaveChangesAsync();

            return new OperationResult<User>
            {
                Success = true,
                Data = newUser,
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new OperationResult<User>
            {
                Success = false,
                ErrorMessage = "Signup failed due to an internal error"
            };
        }
    }
}