using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class WorkshopResourceFromEntityAssembler
{
    public static WorkshopResource ToResourceFromEntity(Workshop entity)
    {
        return new WorkshopResource(
            entity.Id?.Value ?? System.Guid.Empty,
            entity.OwnerId?.Value ?? System.Guid.Empty,
            entity.BusinessName,
            entity.BrandName,
            entity.TaxId.Value,
            entity.MileageIntervalConfig
        );
    }
}
