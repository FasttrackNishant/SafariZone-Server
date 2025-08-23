using AuthService.Core.Interfaces;

namespace AuthService.Core.Services;

public class TouristAuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;

    public TouristAuthService(IUserRepository users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    public async Task<string?> AuthenticateAsync(string email, string password)
    {
        var user = await _users.GetByEmailAsync(email);
        if (user == null) return null;

        if (user.PasswordHash != password) return null;

        var claims = new[] { new System.Security.Claims.Claim("role", user.Role.RoleName) };
        return _tokens.CreateInternalJwt(user.UserId.ToString(), claims);
    }
}