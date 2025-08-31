using AuthService.Core.Entities;

namespace AuthService.Core.Interfaces;

public interface IProfileRepository
{
    Task<Profile?> GetProfileByUserIdAsync(int userId);
    Task<List<Profile>> GetAllProfilesAsync();
    Task AddProfileAsync(Profile profile);
    Task UpdateProfileAsync(Profile profile);
    Task<bool> ProfileExistsAsync(int userId);
    Task<string> GetUserProfileName(int profileId);
}