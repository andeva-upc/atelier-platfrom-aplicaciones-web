using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record UserId
{
    public UserId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("core.error.userId.required");
        }
        Value = value;
    }

    public Guid Value { get; init; }
}
