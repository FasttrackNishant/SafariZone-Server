using System.Security.Claims;

namespace AuthService.Core.Entities;

public class AadUser
{
    public string Subject { get; set; } = string.Empty;    // oid / sub
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public IEnumerable<Claim> AllClaims { get; set; } = [];
}
