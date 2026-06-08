using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record SubscriptionPlanId
{
    public SubscriptionPlanId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("core.error.subscriptionPlanId.required");
        }
        Value = value;
    }

    public Guid Value { get; init; }
}
