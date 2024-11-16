using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);

    Task<User> GetByEmailAsync(string email);

    Task UpdateLastLoginAsync(Guid id);
}
