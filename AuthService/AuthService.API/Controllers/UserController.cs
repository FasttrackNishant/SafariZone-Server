using AuthService.Core.DTO;
using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SafariZone.Server.Common.Response;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository userRepository;

    public UserController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> SignUpUser([FromBody] SignUpRequestDto request)
    {
        try
        {
            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                RoleId = request.RoleId,
                HasAgreedTerms = request.HasAgreedTerms,
                TermsVersion = request.TermsVersion,
                TermsAgreedOn = DateTime.UtcNow
            };

            var result = await userRepository.SignupUser(newUser);

            if (result.Success)
            {
                return Ok(ApiResponse<string>.Created("User created successfully"));
            }
            else
            {
                return BadRequest(ApiResponse<string>.Fail(result.ErrorMessage));
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<string>.Fail($"Something went wrong: {ex.Message}"));
        }
    }
}