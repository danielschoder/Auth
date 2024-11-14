using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface ITokenProvider
{
    string Create(User user);
}
