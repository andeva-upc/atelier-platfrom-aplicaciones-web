using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class VehicleResourceFromAggregateAssembler
{
    public static VehicleResource ToResourceFromAggregate(Vehicle aggregate)
    {
        return new VehicleResource(
            aggregate.Id,
            aggregate.PlateNumber,
            aggregate.Vin,
            aggregate.Year,
            aggregate.Brand,
            aggregate.Model
        );
    }
}
