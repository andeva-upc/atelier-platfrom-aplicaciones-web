using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record UpdateObd2DeviceCommand(Obd2DeviceId Id, string MacAddress);
