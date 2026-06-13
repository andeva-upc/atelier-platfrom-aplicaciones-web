using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class Obd2DeviceResourceFromEntityAssembler
{
    public static Obd2DeviceResource ToResourceFromEntity(Obd2Device entity)
    {
        return new Obd2DeviceResource(
            entity.Id?.Value ?? System.Guid.Empty,
            entity.BranchId?.Value ?? System.Guid.Empty,
            entity.MacAddress,
            entity.Status?.Value ?? string.Empty,
            entity.LastPing
        );
    }
}
