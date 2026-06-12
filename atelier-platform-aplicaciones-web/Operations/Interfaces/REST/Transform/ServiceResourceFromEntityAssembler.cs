using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;

public static class ServiceResourceFromEntityAssembler
{
    public static ServiceResource ToResourceFromEntity(Service entity)
    {
        return new ServiceResource(
            entity.Id.Value,
            entity.BranchId.Value,
            entity.Name,
            entity.Price.Amount
        );
    }
}
