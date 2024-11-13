using Formula1.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Formula1.Infrastructure.Persistence.Configurations;

public class DriverConfiguration
{
    public static void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(1023);
        builder.Property(e => e.WikipediaUrl).HasMaxLength(1023);
    }
}
