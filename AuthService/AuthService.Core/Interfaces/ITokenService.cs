using System.Security.Claims;
namespace AuthService.Core.Interfaces;

public interface ITokenService
{
    string CreateInternalJwt(string subject, IEnumerable<Claim> claims);
    int GetExpiryMinutes();
}