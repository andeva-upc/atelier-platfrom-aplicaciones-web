using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using atelier_platform_aplicaciones_web.Operations.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options, AuditableEntityInterceptor auditableEntityInterceptor, DispatchDomainEventsInterceptor dispatchDomainEventsInterceptor) : DbContext(options)
{
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Obd2Device> Obd2Devices { get; set; }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        // and Dispatch domain events interceptor for all IHasDomainEvents implementations
        builder.AddInterceptors(auditableEntityInterceptor, dispatchDomainEventsInterceptor);
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply Operations Context Configuration
        builder.ApplyOperationsConfiguration();
        
        // Apply IAM Context Configuration
        builder.ApplyIamConfiguration();

        // Apply Core Context Configuration
        builder.ApplyCoreConfiguration();
        
        // Apply Billing Context Configuration
        builder.ApplyBillingConfiguration();
        
        // Apply Inventory Context Configuration
        builder.ApplyInventoryConfiguration();

        // Apply IoT Bounded Context Configuration
        builder.ApplyIotConfiguration();
        
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
