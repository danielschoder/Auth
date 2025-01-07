using Auth.Application.Interfaces;
using Auth.Contracts.DTOs;
using Auth.Domain.Common.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class UserRepository(
    ApplicationDbContext context,
    IDateTimeProvider dateTimeProvider
    ) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        user.CreatedDateTime = user.LastLoginDateTime = _dateTimeProvider.UtcNow;
        await _context.AUTH_Users.AddAsync(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await _context.AUTH_Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task UpdateUserAsync(UserUpdateDto userUpdateDto, CancellationToken cancellationToken)
        => await _context.AUTH_Users
            .Where(u => u.Id == userUpdateDto.Id)
            .ExecuteUpdateAsync(u => u.SetProperty(x => x.Email, userUpdateDto.NewEmail), cancellationToken);

    public async Task UpdateLastLoginAsync(Guid id, CancellationToken cancellationToken)
        => await _context.AUTH_Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u.SetProperty(x => x.LastLoginDateTime, _dateTimeProvider.UtcNow), cancellationToken);
}
