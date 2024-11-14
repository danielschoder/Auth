using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        => await _context.AUTH_Users.FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await _context.AUTH_Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
