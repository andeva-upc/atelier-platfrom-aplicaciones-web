using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class TelemetrySnapshotResourceFromAggregateAssembler
{
    public static TelemetrySnapshotResource ToResourceFromAggregate(TelemetrySnapshot aggregate)
    {
        return new TelemetrySnapshotResource(
            aggregate.Id,
            aggregate.Obd2DeviceRegistrationId,
            aggregate.BranchId,
            aggregate.Rpm,
            aggregate.Temperature,
            aggregate.SpeedKmh,
            aggregate.OdometerKm,
            aggregate.FuelLevelPercent,
            aggregate.CreatedAt
        );
    }
}
