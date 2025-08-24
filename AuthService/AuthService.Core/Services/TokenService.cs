using AuthService.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AuthService.Core.Services;

public class TokenService : ITokenService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _key;
    private readonly int _expiryMinutes;

    public TokenService(IConfiguration config)
    {
        _issuer = config["InternalJwt:Issuer"]!;
        _audience = config["InternalJwt:Audience"]!;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["InternalJwt:Key"]!));
        _expiryMinutes = int.Parse(config["InternalJwt:ExpiryMinutes"]!);
    }

    public string CreateInternalJwt(string subject, IEnumerable<Claim> claims)
    {
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public int GetExpiryMinutes() => _expiryMinutes;

}