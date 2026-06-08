using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface ITelemetrySnapshotRepository : IBaseRepository<TelemetrySnapshot>
{
    Task<TelemetrySnapshot?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TelemetrySnapshot?> FindLatestByRegistrationIdAsync(Guid registrationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TelemetrySnapshot>> FindByRegistrationIdAndDateRangeAsync(Guid registrationId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
}
