using Auth.Contracts.Extensions;

namespace Auth.Contracts.DTOs;

public class RegisterDto()
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string Validate()
    {
        Email = Email?.Trim().ToLower();
        return new Dictionary<Func<bool>, string>
        {
            { () => string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Password), "Please provide an email and a password." },
            { () => string.IsNullOrWhiteSpace(Email), "Please provide an email." },
            { () => string.IsNullOrWhiteSpace(Password), "Please provide a password." },
            { () => Password.Contains(' '), "Password must not contain space characters." }
        }
        .Error();
    }
}
