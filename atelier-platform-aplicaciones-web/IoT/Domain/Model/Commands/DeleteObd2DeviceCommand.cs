using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record DeleteObd2DeviceCommand(Obd2DeviceId Id);
