using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record OwnerId
{
    public OwnerId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("core.error.ownerId.required");
        }
        Value = value;
    }

    public Guid Value { get; init; }
}
