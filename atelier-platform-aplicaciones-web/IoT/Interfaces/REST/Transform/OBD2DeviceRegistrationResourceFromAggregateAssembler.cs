using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class OBD2DeviceRegistrationResourceFromAggregateAssembler
{
    public static OBD2DeviceRegistrationResource ToResourceFromAggregate(OBD2DeviceRegistration aggregate)
    {
        return new OBD2DeviceRegistrationResource(
            aggregate.Id,
            aggregate.Obd2DeviceId,
            aggregate.BranchId,
            aggregate.VehicleId,
            aggregate.Status.ToString(),
            aggregate.CreatedAt,
            aggregate.DeletedAt
        );
    }
}
