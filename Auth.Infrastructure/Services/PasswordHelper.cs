using Auth.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Services;

public class PasswordHelper : IPasswordHelper
{
    private readonly PasswordHasher<object> passwordHasher = new();

    public string HashPassword(string password)
        => passwordHasher.HashPassword(null, password);

    public bool PasswordIsVerified(string hashedPassword, string password)
        => passwordHasher.VerifyHashedPassword(null, hashedPassword, password)
            == PasswordVerificationResult.Success;
}