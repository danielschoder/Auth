namespace Auth.Application.Interfaces;

public interface IPasswordHelper
{
    string HashPassword(string password);
    bool PasswordIsVerified(string hashedPassword, string password);
}
