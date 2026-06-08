using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record WorkshopId
{
    public WorkshopId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("core.error.workshopId.required");
        }
        Value = value;
    }

    public Guid Value { get; init; }
}
