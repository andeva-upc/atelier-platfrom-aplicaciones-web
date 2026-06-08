using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

/// <summary>
///     EF Core interceptor that automatically populates audit timestamps on any entity
///     that implements <see cref="IAuditableEntity"/>, and the user identifier on any
///     entity that implements <see cref="IUserAuditableEntity"/>.
/// </summary>
public sealed class AuditableEntityInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAuditTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditTimestamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAuditTimestamps(DbContext? context)
    {
        if (context is null) return;

        var now = DateTimeOffset.UtcNow;
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid? currentUserId = Guid.TryParse(userIdClaim, out var parsedId) ? parsedId : null;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                if (entry.Entity is IUserAuditableEntity userEntityModified)
                {
                    userEntityModified.UpdatedBy = currentUserId;
                }
            }
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt ??= now;
                if (entry.Entity is IUserAuditableEntity userEntityAdded)
                {
                    userEntityAdded.CreatedBy ??= currentUserId;
                }
            }
        }
    }
}
