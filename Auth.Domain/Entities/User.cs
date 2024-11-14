namespace Auth.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime LastLoginDateTime { get; set; }
}
