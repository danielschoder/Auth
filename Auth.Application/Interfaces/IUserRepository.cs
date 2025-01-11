using Auth.Contracts.DTOs;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);

    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task UpdateUserAsync(
        Guid id,
        UserUpdateDto userUpdateDto,
        CancellationToken cancellationToken);

    Task UpdateLastLoginAsync(Guid id, CancellationToken cancellationToken);
}
