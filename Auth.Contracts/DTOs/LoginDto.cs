using Auth.Contracts.Extensions;

namespace Auth.Contracts.DTOs;

public class LoginDto()
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string Validate()
    {
        Email = Email?.Trim().ToLower();
        Password = Password?.Trim();
        return new Dictionary<Func<bool>, string>
        {
            { () => string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Password), "Please provide an email and a password." },
            { () => string.IsNullOrWhiteSpace(Email), "Please provide an email." },
            { () => string.IsNullOrWhiteSpace(Password), "Please provide a password." }
        }
        .Error();
    }
}
