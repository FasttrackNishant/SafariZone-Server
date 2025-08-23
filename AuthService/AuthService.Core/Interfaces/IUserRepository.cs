using AuthService.Core.Entities;

namespace AuthService.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}