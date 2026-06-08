using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record BranchSubscriptionId
{
    public BranchSubscriptionId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("core.error.branchSubscriptionId.required");
        }
        Value = value;
    }

    public Guid Value { get; init; }
}
