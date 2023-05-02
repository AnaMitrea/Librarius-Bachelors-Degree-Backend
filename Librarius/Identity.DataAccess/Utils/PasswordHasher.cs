using System.Security.Cryptography;

namespace Identity.DataAccess.Utils;

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public static string HashPassword(string password, string pepper)
    {
        var salt = new byte[SaltSize];
        RandomNumberGenerator.Fill(salt);
        
        var pbkdf2 = new Rfc2898DeriveBytes(password + pepper, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(HashSize);

        var hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string hashedPassword, string pepper)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);
        
        var pbkdf2 = new Rfc2898DeriveBytes(password + pepper, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(HashSize);

        for (var i = 0; i < HashSize; i++)
            if (hashBytes[i + SaltSize] != hash[i])
                return false;

        return true;
    }
}