using AuthService.Core.Helpers;
using AuthService.Core.Interfaces;

namespace AuthService.Core.Services;

public class TouristAuthService(IUserRepository users, ITokenService tokens, IProfileRepository profiles) : IAuthService
{
    public async Task<string?> AuthenticateAsync(string email, string password)
    {
        var user = await users.GetByEmailAsync(email);
        if (user == null) return null;

        var userName = await profiles.GetUserProfileName(user.UserId);


        if (!PasswordHelper.VerifyPassword(password, user.PasswordHash, user.Salt))
            return null;

        var claims = new[]
        {
            new System.Security.Claims.Claim("role", user.Role.RoleName),
            new System.Security.Claims.Claim("email", user.Email),
            new System.Security.Claims.Claim("name", userName),
        };
        return tokens.CreateInternalJwt(user.UserId.ToString(), claims);
    }
}