using Microsoft.AspNetCore.Identity;

namespace Auth.Helpers;

public class PasswordHelper
{
    private static readonly PasswordHasher<object> passwordHasher = new PasswordHasher<object>();

    public static string HashPassword(string password)
    {
        var hashedPassword = passwordHasher.HashPassword(null, password);
        return hashedPassword;
    }

    public static bool VerifyPassword(string hashedPassword, string password)
    {
        var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}
