using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Auth.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
    : DbContext(options)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

    public DbSet<User> AUTH_Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserConfiguration.Configure(modelBuilder.Entity<User>());

        base.OnModelCreating(modelBuilder);
    }
}
