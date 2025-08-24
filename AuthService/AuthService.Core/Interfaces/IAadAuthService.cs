using System.Security.Claims;
using AuthService.API.DTO;
using AuthService.Core.DTO;
using AuthService.Core.Entities;

namespace AuthService.Core.Interfaces;

public interface IAadAuthService
{
    Task<ExchangeResponse?> ExchangeTokenAsync(ClaimsPrincipal aadPrincipal);
}