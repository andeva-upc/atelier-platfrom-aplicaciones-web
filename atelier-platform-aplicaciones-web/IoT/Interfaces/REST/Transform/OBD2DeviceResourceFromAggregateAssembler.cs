using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class OBD2DeviceResourceFromAggregateAssembler
{
    public static OBD2DeviceResource ToResourceFromAggregate(OBD2Device aggregate)
    {
        return new OBD2DeviceResource(
            aggregate.Id,
            aggregate.BranchId.Value,
            aggregate.MacAddress,
            aggregate.Status.ToString(),
            aggregate.LastPing
        );
    }
}
