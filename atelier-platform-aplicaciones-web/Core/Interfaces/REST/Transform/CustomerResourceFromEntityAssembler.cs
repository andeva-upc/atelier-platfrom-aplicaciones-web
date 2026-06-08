using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class CustomerResourceFromEntityAssembler
{
    public static CustomerResource ToResourceFromEntity(Customer entity)
    {
        return new CustomerResource(
            entity.Id?.Value ?? System.Guid.Empty,
            entity.UserId?.Value ?? System.Guid.Empty,
            entity.IsCorporate,
            entity.Name?.FirstName,
            entity.Name?.LastName,
            entity.BusinessName,
            entity.Document?.DocumentType.ToString() ?? string.Empty,
            entity.Document?.DocumentNumber ?? string.Empty,
            entity.Phone?.Value ?? string.Empty
        );
    }
}
