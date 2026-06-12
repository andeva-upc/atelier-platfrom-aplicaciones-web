using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;

public static class CreateServiceCommandFromResourceAssembler
{
    public static CreateServiceCommand ToCommandFromResource(CreateServiceResource resource)
    {
        return new CreateServiceCommand(
            new BranchId(resource.BranchId),
            resource.Name,
            new Money(resource.Price)
        );
    }
}
