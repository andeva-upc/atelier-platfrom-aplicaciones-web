using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class CreateOBD2DeviceCommandFromResourceAssembler
{
    public static CreateOBD2DeviceCommand ToCommandFromResource(CreateOBD2DeviceResource resource)
    {
        return new CreateOBD2DeviceCommand(resource.BranchId, resource.MacAddress);
    }
}
