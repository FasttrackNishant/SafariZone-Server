using System.Security.Claims;
using AuthService.API.DTO;
using AuthService.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafariZone.Server.Common.Response;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _touristAuth;
    private readonly IAadAuthService _aadAuthService;

    public AuthController(IAuthService touristAuth, IAadAuthService aadAuthService)
    {
        _touristAuth = touristAuth;
        _aadAuthService = aadAuthService;
    }

    [HttpPost("login-tourist")]
    public async Task<ActionResult<TokenResponse>> LoginTourist([FromBody] LoginRequest req)
    {
        var token = await _touristAuth.AuthenticateAsync(req.Email, req.Password);
        if (token == null) return Unauthorized(ApiResponse<TokenResponse>.Unauthorized("Invalid credential"));
        var tokenResponse = new TokenResponse { AccessToken = token, ExpiresAt = DateTime.UtcNow.AddMinutes(60) };
        return Ok(ApiResponse<TokenResponse>.Ok(tokenResponse, "Login Successfully"));
    }

    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = "Internal")]
    public ActionResult<ApiResponse<MeResponse>> Me()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var email = User.FindFirstValue(ClaimTypes.Email) ?? "";
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        var response = new MeResponse
        {
            Subject = sub ?? "unknown",
            Email = email,
            Roles = roles
        };

        return Ok(ApiResponse<MeResponse>.Ok(response, "User details retrieved successfully"));
    }


    [HttpPost("exchange")]
    [Authorize(AuthenticationSchemes = "AzureAd")]
    public async Task<ActionResult<ApiResponse<TokenResponse>>> ExchangeFromAad()
    {
        var result = await _aadAuthService.ExchangeTokenAsync(HttpContext.User);
        if (result is null)
            return Unauthorized(ApiResponse<TokenResponse>.Unauthorized("Exchange failed"));
        var tokenResponse = new TokenResponse { AccessToken = result.AccessToken, ExpiresAt = result.ExpiresAt };
        return Ok(ApiResponse<TokenResponse>.Ok(tokenResponse, "Token exchange successful"));
    }

    [HttpPost("verify")]
    [Authorize(AuthenticationSchemes = "Internal")]
    public IActionResult VerifyToken()
    {
        return Ok(new { Verified = true });
    }
}