using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Configurations;

public class UserConfiguration
{
    public static void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(1023);
        builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(1023);
    }
}
