using System;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record Obd2DeviceRegistrationId(Guid Value)
{
    public Obd2DeviceRegistrationId() : this(Guid.NewGuid()) {}
}
