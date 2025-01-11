using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Configurations;

public class UserConfiguration
{
    public static void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email).HasMaxLength(1023).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(1023);
        builder.Property(e => e.NickName).HasMaxLength(1023);
        builder.Property(e => e.PasswordHash).HasMaxLength(1023).IsRequired();

        builder.HasIndex(e => e.Email).IsUnique();
    }
}
