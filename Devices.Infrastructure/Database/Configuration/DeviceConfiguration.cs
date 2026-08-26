using Devices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Devices.Infrastructure.Database.Configuration;

public sealed class DeviceConfiguration
    : IEntityTypeConfiguration<Device>
{
    public void Configure(
        EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("devices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Brand)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.State)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CreationTime)
            .IsRequired();

        builder.HasIndex(x => x.Brand);

        builder.HasIndex(x => x.State);
    }
}