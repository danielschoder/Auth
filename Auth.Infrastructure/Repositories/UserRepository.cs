using Auth.Application.Interfaces;
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

    public async Task AddAsync(User user)
    {
        user.CreatedDateTime = user.LastLoginDateTime = _dateTimeProvider.UtcNow;
        await _context.AUTH_Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByEmailAsync(string email)
        => await _context.AUTH_Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task UpdateLastLoginAsync(Guid id)
        => await _context.AUTH_Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u.SetProperty(x => x.LastLoginDateTime, _dateTimeProvider.UtcNow));
}
