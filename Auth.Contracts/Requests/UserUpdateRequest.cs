
using Auth.Domain.Entities;

namespace Auth.Contracts.Requests;

public record UserUpdateRequest(string Email, string Name, string NickName)
{
    public User AsUser(Guid id)
        => new()
        {
            Id = id,
            Email = Email,
            Name = Name,
            NickName = NickName
        };
}
