using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class CreateBranchCommandFromResourceAssembler
{
    public static CreateBranchCommand ToCommandFromResource(CreateBranchResource resource)
    {
        return new CreateBranchCommand(
            new WorkshopId(resource.WorkshopId),
            resource.Code,
            resource.Name,
            new Address(resource.Address),
            new Phone(resource.Phone)
        );
    }
}
