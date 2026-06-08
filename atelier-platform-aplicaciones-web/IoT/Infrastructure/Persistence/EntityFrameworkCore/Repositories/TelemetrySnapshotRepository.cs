using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TelemetrySnapshotRepository : BaseRepository<TelemetrySnapshot>, ITelemetrySnapshotRepository
{
    public TelemetrySnapshotRepository(AppDbContext context) : base(context) {}

    public async Task<TelemetrySnapshot?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TelemetrySnapshot>().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<TelemetrySnapshot?> FindLatestByRegistrationIdAsync(Guid registrationId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TelemetrySnapshot>()
            .Where(s => s.Obd2DeviceRegistrationId == registrationId)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TelemetrySnapshot>> FindByRegistrationIdAndDateRangeAsync(Guid registrationId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TelemetrySnapshot>()
            .Where(s => s.Obd2DeviceRegistrationId == registrationId && s.CreatedAt >= from && s.CreatedAt <= to)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
