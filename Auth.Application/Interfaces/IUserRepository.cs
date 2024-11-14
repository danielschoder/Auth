using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    Task AddUserAsync(User user, CancellationToken cancellationToken);
}
