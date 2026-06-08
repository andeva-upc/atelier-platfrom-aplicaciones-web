using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class CreateWorkshopCommandFromResourceAssembler
{
    public static CreateWorkshopCommand ToCommandFromResource(CreateWorkshopResource resource)
    {
        return new CreateWorkshopCommand(
            new OwnerId(resource.OwnerId),
            resource.BusinessName,
            resource.BrandName,
            resource.TaxId,
            resource.MileageIntervalConfig
        );
    }
}
