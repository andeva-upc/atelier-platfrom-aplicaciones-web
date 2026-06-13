using System;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record Obd2DeviceResource(
    Guid Id,
    Guid BranchId,
    string MacAddress,
    string Status,
    DateTimeOffset? LastPing
);
