using System;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class UpdateObd2DeviceCommandFromResourceAssembler
{
    public static UpdateObd2DeviceCommand ToCommandFromResource(Guid id, UpdateObd2DeviceResource resource)
    {
        return new UpdateObd2DeviceCommand(
            new Obd2DeviceId(id),
            resource.MacAddress
        );
    }
}
