using AuthService.API.DTO;
using AuthService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _touristAuth;

    public AuthController(IAuthService touristAuth)
    {
        _touristAuth = touristAuth;
    }

    
    public ActionResult Test()
    {
        return Ok("Hi from Auth Service");
    }
    

   [HttpPost("login-tourist")]
    public async Task<ActionResult<TokenResponse>> LoginTourist([FromBody] LoginRequest req)
    {
        var token = await _touristAuth.AuthenticateAsync(req.Email, req.Password);

        if (token == null) return Unauthorized("Invalid credentials");

        return new TokenResponse { AccessToken = token, ExpiresAt = DateTime.UtcNow.AddMinutes(60) };
    }
}