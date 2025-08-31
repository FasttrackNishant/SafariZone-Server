namespace AuthService.Core.Entities;

public class Profile
{
    public int ProfileId { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public int GenderId { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public string ZipCode { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public bool IsProfileComplete { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}