namespace AuthService.API.DTO;

public class MeResponse
{
    public string Subject { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}