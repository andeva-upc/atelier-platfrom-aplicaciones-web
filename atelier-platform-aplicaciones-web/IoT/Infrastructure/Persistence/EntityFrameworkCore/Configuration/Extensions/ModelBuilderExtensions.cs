using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIotConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Vehicle>().HasQueryFilter(v => v.DeletedAt == null);
        builder.Entity<OBD2Device>().HasQueryFilter(d => d.DeletedAt == null);
        builder.Entity<OBD2DeviceRegistration>().HasQueryFilter(r => r.DeletedAt == null);
        builder.Entity<VehicleRegistration>().HasQueryFilter(r => r.DeletedAt == null);

        // Vehicles
        builder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Id).ValueGeneratedNever();
            entity.Property(v => v.PlateNumber).IsRequired();
            entity.Property(v => v.Vin).IsRequired();
            entity.HasIndex(v => v.Vin).IsUnique();
            entity.Property(p => p.Version).IsConcurrencyToken();
        });

        // Vehicle registrations
        builder.Entity<VehicleRegistration>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).ValueGeneratedNever();
            entity.Property(r => r.UserId).IsRequired();
            entity.Property(r => r.VehicleId)
                .HasConversion(vo => vo.Value, value => new VehicleId(value))
                .IsRequired();
            entity.Property(r => r.Status)
                .HasConversion(
                    status => status == VehicleRegistrationStatus.ACTIVE ? "ACTIVE" : "PREVIOUS",
                    value => value.ToUpper() == "ACTIVE" ? VehicleRegistrationStatus.ACTIVE : VehicleRegistrationStatus.PREVIOUS)
                .IsRequired();
        });

        // OBD2 Devices
        builder.Entity<OBD2Device>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id).ValueGeneratedNever();
            entity.Property(d => d.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(d => d.MacAddress).IsRequired();
            entity.HasIndex(d => d.MacAddress).IsUnique();
            entity.Property(d => d.Status)
                .HasConversion(
                    status => status == OBD2DeviceStatus.AVAILABLE ? "AVAILABLE" :
                        status == OBD2DeviceStatus.LINKED ? "LINKED" : "NOT_AVAILABLE",
                    value => value.ToUpper() == "AVAILABLE" ? OBD2DeviceStatus.AVAILABLE :
                        value.ToUpper() == "LINKED" ? OBD2DeviceStatus.LINKED : OBD2DeviceStatus.NOT_AVAILABLE)
                .IsRequired();
            entity.Property(p => p.Version).IsConcurrencyToken();
        });

        // OBD2 Device Registrations
        builder.Entity<OBD2DeviceRegistration>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).ValueGeneratedNever();
            entity.Property(r => r.Obd2DeviceId).IsRequired();
            entity.Property(r => r.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(r => r.VehicleId)
                .HasConversion(vo => vo.Value, value => new VehicleId(value))
                .IsRequired();
            entity.Property(r => r.Status)
                .HasConversion(
                    status => status == OBD2DeviceRegistrationStatus.ACTIVE ? "ACTIVE" : "INACTIVE",
                    value => value.ToUpper() == "ACTIVE" ? OBD2DeviceRegistrationStatus.ACTIVE : OBD2DeviceRegistrationStatus.INACTIVE)
                .IsRequired();
        });

        // Telemetry Snapshots
        builder.Entity<TelemetrySnapshot>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedNever();
            entity.Property(s => s.Obd2DeviceRegistrationId).IsRequired();
            entity.Property(s => s.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
        });

        // Dtc Alerts
        builder.Entity<DtcAlert>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedNever();
            entity.Property(a => a.TelemetrySnapshotId).IsRequired();
            entity.Property(a => a.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(a => a.DtcCode)
                .HasConversion(vo => vo.Value, value => new DtcCode(value))
                .IsRequired();
            entity.Property(a => a.Severity)
                .HasConversion(
                    severity => severity == DtcSeverity.LOW ? "LOW" :
                        severity == DtcSeverity.MEDIUM ? "MEDIUM" :
                        severity == DtcSeverity.HIGH ? "HIGH" : "CRITICAL",
                    value => value.ToUpper() == "LOW" ? DtcSeverity.LOW :
                        value.ToUpper() == "MEDIUM" ? DtcSeverity.MEDIUM :
                        value.ToUpper() == "HIGH" ? DtcSeverity.HIGH : DtcSeverity.CRITICAL)
                .IsRequired();
        });
    }
}
