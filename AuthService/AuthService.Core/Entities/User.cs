using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Core.Entities;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? AadId { get; set; }
    public string? PasswordHash { get; set; } 
    public string? Salt { get; set; } 
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public bool HasAgreedTerms { get; set; }
    public DateTime? TermsAgreedOn { get; set; }
    public string? TermsVersion { get; set; }
    public Role Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}