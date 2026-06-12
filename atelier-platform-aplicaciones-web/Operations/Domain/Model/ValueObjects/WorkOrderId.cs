using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record WorkOrderId
{
    private const string NotNullUuidMessage = "operations.error.workOrderId.required";

    public Guid Value { get; init; }

    public WorkOrderId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}
