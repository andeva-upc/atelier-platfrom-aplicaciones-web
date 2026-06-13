using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class Obd2DeviceRegistrationResourceFromEntityAssembler
{
    public static Obd2DeviceRegistrationResource ToResourceFromEntity(Obd2DeviceRegistration entity)
    {
        return new Obd2DeviceRegistrationResource(
            entity.Id.Value,
            entity.Obd2DeviceId.Value,
            entity.BranchId.Value,
            entity.VehicleId.Value,
            entity.Status.Value,
            entity.CreatedAt
        );
    }
}
