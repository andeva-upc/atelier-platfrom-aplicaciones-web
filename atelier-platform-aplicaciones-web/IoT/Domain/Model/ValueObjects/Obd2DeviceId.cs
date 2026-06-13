using System;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record Obd2DeviceId
{
    private const string NotNullUuidMessage = "iot.error.obd2DeviceId.required";

    public Guid Value { get; init; }

    public Obd2DeviceId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}
