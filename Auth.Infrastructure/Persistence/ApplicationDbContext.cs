using Formula1.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Auth.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
    : DbContext(options), IApplicationDbContext
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

    public DbSet<User> FORMULA1_Circuits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DriverConfiguration.Configure(modelBuilder.Entity<User>());

        base.OnModelCreating(modelBuilder);
    }
}
