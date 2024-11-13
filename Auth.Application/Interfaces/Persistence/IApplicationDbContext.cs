using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Interfaces.Persistence;

public interface IApplicationDbContext
{
    public DbSet<User> AUTH_Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
