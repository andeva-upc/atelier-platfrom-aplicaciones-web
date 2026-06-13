using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class CreateObd2DeviceCommandFromResourceAssembler
{
    public static CreateObd2DeviceCommand ToCommandFromResource(CreateObd2DeviceResource resource)
    {
        return new CreateObd2DeviceCommand(
            new BranchId(resource.BranchId),
            resource.MacAddress
        );
    }
}
