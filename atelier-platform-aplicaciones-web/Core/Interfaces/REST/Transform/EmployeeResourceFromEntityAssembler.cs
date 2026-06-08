using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class EmployeeResourceFromEntityAssembler
{
    public static EmployeeResource ToResourceFromEntity(Employee entity)
    {
        return new EmployeeResource(
            entity.Id?.Value ?? System.Guid.Empty,
            entity.UserId?.Value ?? System.Guid.Empty,
            entity.Name?.FirstName ?? string.Empty,
            entity.Name?.LastName ?? string.Empty,
            entity.Document?.DocumentType.ToString() ?? string.Empty,
            entity.Document?.DocumentNumber ?? string.Empty,
            entity.Phone?.Value ?? string.Empty
        );
    }
}
