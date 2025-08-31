using AuthService.Core.Entities;
using AuthService.Core.Interfaces;

namespace AuthService.Infrastructure.Persistence;

public class ProfileRepository(AuthDbContext _db) : IProfileRepository
{
    public Task<Profile?> GetProfileByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Profile>> GetAllProfilesAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddProfileAsync(Profile profile)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProfileAsync(Profile profile)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ProfileExistsAsync(int userId)
    {
        throw new NotImplementedException();
    }
    
    public async Task<string> GetUserProfileName(int profileId)
    {
        var profile = await _db.Profiles.FindAsync(profileId);
        if (profile == null) return string.Empty;

        return $"{profile.FirstName} {profile.LastName}";
    }
}