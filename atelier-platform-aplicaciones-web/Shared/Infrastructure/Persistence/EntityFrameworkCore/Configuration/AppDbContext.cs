using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using atelier_platform_aplicaciones_web.Operations.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options, AuditableEntityInterceptor auditableEntityInterceptor, DispatchDomainEventsInterceptor dispatchDomainEventsInterceptor) : DbContext(options)
{

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
