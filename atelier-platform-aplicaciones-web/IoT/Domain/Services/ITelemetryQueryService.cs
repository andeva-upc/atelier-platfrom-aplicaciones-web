using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Services;

public interface ITelemetryQueryService
{
    Task<TelemetrySnapshot?> Handle(GetLatestTelemetryByDeviceIdQuery query);
    Task<IEnumerable<TelemetrySnapshot>> Handle(GetTelemetryHistoryByDeviceIdQuery query);
}
