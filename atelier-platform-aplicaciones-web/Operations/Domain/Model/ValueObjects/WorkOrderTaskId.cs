using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record WorkOrderTaskId
{
    private const string NotNullUuidMessage = "operations.error.workOrderTaskId.required";

    public Guid Value { get; init; }

    public WorkOrderTaskId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}
