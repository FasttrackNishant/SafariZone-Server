namespace AuthService.Core.DTO;

public class SignUpRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public bool HasAgreedTerms { get; set; }
    public string? TermsAgreedOn { get; set; }
    public string? TermsVersion { get; set; }
}