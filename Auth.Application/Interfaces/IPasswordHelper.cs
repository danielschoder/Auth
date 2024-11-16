namespace Auth.Application.Interfaces;

public interface IPasswordHelper
{
    string HashPassword(string password);
    bool IsPasswordVerified(string hashedPassword, string password);
}
