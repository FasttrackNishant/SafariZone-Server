namespace AuthService.Core.Interfaces;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(string email, string password);
}