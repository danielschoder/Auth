using Auth.Contracts.Requests;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface IUserRepository
{
    Task<bool> AddAsync(User user, CancellationToken cancellationToken);

    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> UpdateUserAsync(
        Guid id,
        UserUpdateRequest userUpdateDto,
        CancellationToken cancellationToken);

    Task<bool> UpdateLastLoginAsync(Guid id, CancellationToken cancellationToken);
}
