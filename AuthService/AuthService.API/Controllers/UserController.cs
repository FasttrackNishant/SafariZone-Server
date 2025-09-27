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
    [Route("signup-user")]
    public async Task<ActionResult<ApiResponse<string>>> SignUpUser([FromBody] SignUpRequestDto request)
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
                return ApiResponse<string>.Created(null,message: "User created successfully");
            }
            else
            {
                return ApiResponse<string>.Fail(result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            return ApiResponse<string>.Fail($"Something went wrong: {ex.Message}");
        }
    }
}