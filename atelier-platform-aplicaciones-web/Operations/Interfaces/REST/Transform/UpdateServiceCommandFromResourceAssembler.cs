using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;

public static class UpdateServiceCommandFromResourceAssembler
{
    public static UpdateServiceCommand ToCommandFromResource(Guid serviceId, UpdateServiceResource resource)
    {
        return new UpdateServiceCommand(
            new ServiceId(serviceId),
            resource.Name,
            new Money(resource.Price)
        );
    }
}
