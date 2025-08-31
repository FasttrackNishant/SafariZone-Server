namespace AuthService.Core.Entities;

public class Gender
{
    public int GenderId { get; set; }
    public string GenderName { get; set; } = string.Empty; // Male, Female, Other
}