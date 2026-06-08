using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class LinkDeviceCommandFromResourceAssembler
{
    public static LinkDeviceCommand ToCommandFromResource(LinkDeviceResource resource)
    {
        return new LinkDeviceCommand(resource.Obd2DeviceId, resource.VehicleId, resource.BranchId);
    }
}
