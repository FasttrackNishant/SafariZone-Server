using System.Security.Cryptography;

namespace AuthService.Core.Helpers;

public static class PasswordHelper
{
    public static (string Hash, string Salt) HashPassword(string password)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(16);

        var key = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256);
        byte[] hashBytes = key.GetBytes(32);

        return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);

        var key = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 100000, HashAlgorithmName.SHA256);
        byte[] enteredHashBytes = key.GetBytes(32);

        string enteredHash = Convert.ToBase64String(enteredHashBytes);

        return enteredHash == storedHash;
    }
}