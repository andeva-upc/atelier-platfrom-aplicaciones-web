using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class DtcAlertResourceFromAggregateAssembler
{
    public static DtcAlertResource ToResourceFromAggregate(DtcAlert aggregate)
    {
        return new DtcAlertResource(
            aggregate.Id,
            aggregate.TelemetrySnapshotId,
            aggregate.BranchId,
            aggregate.DtcCode.Value,
            aggregate.Description,
            aggregate.Severity.ToString(),
            aggregate.CreatedAt
        );
    }
}
