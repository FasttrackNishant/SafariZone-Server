namespace AuthService.Core.DTO;

public record ExchangeResponse(
    string AccessToken,
    DateTime ExpiresAt,
    string Subject,
    string Email,
    IEnumerable<string> Roles
);