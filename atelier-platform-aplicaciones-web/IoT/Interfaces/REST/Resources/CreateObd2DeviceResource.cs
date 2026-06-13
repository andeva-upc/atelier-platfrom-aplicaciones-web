using System;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record CreateObd2DeviceResource(
    Guid BranchId,
    string MacAddress
);
