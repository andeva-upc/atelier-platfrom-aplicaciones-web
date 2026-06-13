using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class LinkObd2DeviceToVehicleCommandFromResourceAssembler
{
    public static LinkObd2DeviceToVehicleCommand ToCommandFromResource(CreateObd2DeviceRegistrationResource resource)
    {
        return new LinkObd2DeviceToVehicleCommand(
            new Obd2DeviceId(resource.Obd2DeviceId),
            new BranchId(resource.BranchId),
            new VehicleId(resource.VehicleId)
        );
    }
}
