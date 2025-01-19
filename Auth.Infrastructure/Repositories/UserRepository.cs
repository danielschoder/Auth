using Auth.Application.Interfaces;
using Auth.Contracts.Requests;
using Auth.Domain.Common.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class UserRepository(
    ApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IScopedLogger scopedLogger)
    : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IScopedLogger _scopedLogger = scopedLogger;

    public async Task<bool> AddAsync(User user, CancellationToken cancellationToken)
    {
        user.CreatedDateTime = user.LastLoginDateTime = _dateTimeProvider.UtcNow;
        await _context.AUTH_Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await _context.AUTH_Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<bool> UpdateUserAsync(
        Guid id,
        UserUpdateRequest userUpdateRequest,
        CancellationToken cancellationToken)
    {
        _scopedLogger.Log($"id: [{id}], Email: [{userUpdateRequest.Email}]");

        var userEntity = userUpdateRequest.AsUser(id);
        _context.AUTH_Users.Update(userEntity);
        _context.Entry(userEntity).Property(p => p.PasswordHash).IsModified = false;
        _context.Entry(userEntity).Property(p => p.CreatedDateTime).IsModified = false;
        _context.Entry(userEntity).Property(p => p.LastLoginDateTime).IsModified = false;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> UpdateLastLoginAsync(Guid id, CancellationToken cancellationToken)
    {
        await _context.AUTH_Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u.SetProperty(x => x.LastLoginDateTime, _dateTimeProvider.UtcNow), cancellationToken);
        return true;
    }
}
