using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class UpdateWorkshopCommandFromResourceAssembler
{
    public static UpdateWorkshopCommand ToCommandFromResource(Guid id, UpdateWorkshopResource resource)
    {
        return new UpdateWorkshopCommand(
            new WorkshopId(id),
            resource.BusinessName,
            resource.BrandName,
            new TaxId(resource.TaxId),
            resource.MileageIntervalConfig
        );
    }
}
