using AuthService.Core.Entities;

namespace AuthService.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> GetOrCreateAadUserAsync(string aadId, string email, List<string> roles);
    int MapRoleToInternalId(List<string> roles);
}