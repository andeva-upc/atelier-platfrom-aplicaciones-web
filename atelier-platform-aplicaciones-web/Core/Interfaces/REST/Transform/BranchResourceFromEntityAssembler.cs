using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class BranchResourceFromEntityAssembler
{
    public static BranchResource ToResourceFromEntity(Branch entity)
    {
        return new BranchResource(
            entity.Id?.Value ?? System.Guid.Empty,
            entity.WorkshopId?.Value ?? System.Guid.Empty,
            entity.Code,
            entity.Name,
            entity.Address?.Value ?? string.Empty,
            entity.Phone?.Value ?? string.Empty
        );
    }
}
