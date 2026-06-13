using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIotConfiguration(this ModelBuilder builder)
    {
        // Query filter for soft delete
        builder.Entity<Obd2Device>().HasQueryFilter(d => d.DeletedAt == null);

        // Obd2Device mapping
        builder.Entity<Obd2Device>(entity =>
        {
            entity.ToTable("obd2_devices");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new Obd2DeviceId(v))
                .IsRequired();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.MacAddress).IsRequired().HasMaxLength(17);
            entity.HasIndex(e => e.MacAddress).IsUnique();

            entity.Property(e => e.Status)
                .HasConversion(v => v.Value, v => new Obd2DeviceStatus(v))
                .IsRequired().HasMaxLength(20);

            entity.Property(e => e.LastPing);

            entity.Property(e => e.Version).IsConcurrencyToken();
        });

        builder.Entity<Obd2DeviceRegistration>().HasQueryFilter(r => r.DeletedAt == null);

        builder.Entity<Obd2DeviceRegistration>(entity =>
        {
            entity.ToTable("obd2_device_registrations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new Obd2DeviceRegistrationId(v))
                .IsRequired();

            entity.Property(e => e.Obd2DeviceId)
                .HasConversion(v => v.Value, v => new Obd2DeviceId(v))
                .IsRequired();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.VehicleId)
                .HasConversion(v => v.Value, v => new VehicleId(v))
                .IsRequired();

            entity.Property(e => e.Status)
                .HasConversion(v => v.Value, v => new Obd2RegistrationStatus(v))
                .IsRequired().HasMaxLength(20);

            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.DeletedAt);
        });
    }
}
