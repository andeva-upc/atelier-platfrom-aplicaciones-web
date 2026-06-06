using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;

using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<WorkOrder>().HasQueryFilter(w => w.DeletedAt == null);
        builder.Entity<WorkOrderTask>().HasQueryFilter(t => t.DeletedAt == null);
        builder.Entity<WorkOrderTaskProduct>().HasQueryFilter(p => p.DeletedAt == null);

        builder.Entity<WorkOrderTaskProduct>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedNever();
            entity.Property(p => p.ProductId)
                .HasConversion(vo => vo.Value, value => new ProductId(value))
                .IsRequired();
            entity.Property(p => p.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(p => p.Quantity)
                .HasConversion(vo => vo.Value, value => new Quantity(value))
                .IsRequired();
            entity.Property(p => p.UnitPrice)
                .HasConversion(vo => vo.Amount, value => new Money(value))
                .IsRequired();
            entity.Property(p => p.TotalAmount)
                .HasConversion(vo => vo.Amount, value => new Money(value))
                .IsRequired();
            entity.Property(p => p.Version).IsConcurrencyToken();
        });
        
        builder.Entity<WorkOrderTask>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedNever();
            entity.Property(t => t.ServiceId)
                .HasConversion(vo => vo.Value, value => new ServiceId(value))
                .IsRequired();
            entity.Property(t => t.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(t => t.AssignedMechanicId)
                .HasConversion(vo => vo.Value, value => new MechanicId(value))
                .IsRequired();
            entity.Property(t => t.Description)
                .HasConversion(vo => vo.Value, value => new TaskDescription(value))
                .IsRequired()
                .HasColumnType("TEXT");
            entity.Property(t => t.Price)
                .HasConversion(vo => vo.Amount, value => new Money(value))
                .IsRequired();
            entity.Property(t => t.Status)
                .HasConversion(
                    status => status == WorkOrderTaskStatus.Pending ? "PENDING" :
                        status == WorkOrderTaskStatus.Doing ? "DOING" :
                        status == WorkOrderTaskStatus.Completed ? "COMPLETED" : "PENDING",
                    value => value.ToUpper() == "PENDING" ? WorkOrderTaskStatus.Pending :
                        value.ToUpper() == "DOING" ? WorkOrderTaskStatus.Doing :
                        value.ToUpper() == "COMPLETED" ? WorkOrderTaskStatus.Completed : WorkOrderTaskStatus.Pending)
                .IsRequired();
            entity.HasMany(t => t.Products)
                .WithOne()
                .HasForeignKey("WorkOrderTaskId")
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.Version).IsConcurrencyToken();
        });
        
        builder.Entity<WorkOrder>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Id).ValueGeneratedNever();
            entity.Property(w => w.AppointmentId)
                .HasConversion(vo => vo.Value, value => new AppointmentId(value))
                .IsRequired();
            entity.Property(w => w.BranchId)
                .HasConversion(vo => vo.Value, value => new BranchId(value))
                .IsRequired();
            entity.Property(w => w.VehicleId)
                .HasConversion(vo => vo.Value, value => new VehicleId(value))
                .IsRequired();
            entity.Property(w => w.CustomerId)
                .HasConversion(vo => vo.Value, value => new CustomerId(value))
                .IsRequired();
            entity.Property(w => w.InternalNumber)
                .IsRequired();
            entity.Property(w => w.DiagnosticSummary)
                .HasConversion(vo => vo.Value, value => new DiagnosticSummary(value))
                .IsRequired()
                .HasColumnType("TEXT");
            entity.Property(w => w.MileageIn)
                .HasConversion(vo => vo.Value, value => new Mileage(value))
                .IsRequired();
            entity.Property(w => w.TotalAmount)
                .HasConversion(vo => vo.Amount, value => new Money(value))
                .IsRequired();
            entity.Property(w => w.Status)
                .HasConversion(
                    status => status == WorkOrderStatus.Pending ? "PENDING" :
                        status == WorkOrderStatus.InProgress ? "IN_PROGRESS" :
                        status == WorkOrderStatus.Completed ? "COMPLETED" :
                        status == WorkOrderStatus.Paid ? "PAID" : "PENDING",
                    value => value.ToUpper() == "PENDING" ? WorkOrderStatus.Pending :
                        value.ToUpper() == "IN_PROGRESS" ? WorkOrderStatus.InProgress :
                        value.ToUpper() == "COMPLETED" ? WorkOrderStatus.Completed :
                        value.ToUpper() == "PAID" ? WorkOrderStatus.Paid : WorkOrderStatus.Pending)
                .IsRequired();
            entity.HasMany(w => w.Tasks)
                .WithOne()
                .HasForeignKey("WorkOrderId")
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.Version).IsConcurrencyToken();
        });
        
        builder.UseSnakeCaseNamingConvention();
        
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTimeOffset, DateTime>(
                        v => v.UtcDateTime,
                        v => new DateTimeOffset(v, TimeSpan.Zero)));
                }
                else if (property.ClrType == typeof(DateTimeOffset?))
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTimeOffset?, DateTime?>(
                        v => v.HasValue ? v.Value.UtcDateTime : null,
                        v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : null));
                }
            }
        }
    }
}
