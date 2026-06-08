using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class UpdateBranchCommandFromResourceAssembler
{
    public static UpdateBranchCommand ToCommandFromResource(Guid id, UpdateBranchResource resource)
    {
        return new UpdateBranchCommand(
            new BranchId(id),
            resource.Code,
            resource.Name,
            new Address(resource.Address),
            new Phone(resource.Phone)
        );
    }
}
