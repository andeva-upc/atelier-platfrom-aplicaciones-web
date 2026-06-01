using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record ServiceId
{
    private const string NotNullUuidMessage = "operations.error.serviceId.required";

    public Guid Value { get; init; }

    public ServiceId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}