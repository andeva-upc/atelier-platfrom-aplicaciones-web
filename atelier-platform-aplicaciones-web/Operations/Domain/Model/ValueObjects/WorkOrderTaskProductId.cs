using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record WorkOrderTaskProductId
{
    private const string NotNullUuidMessage = "operations.error.workOrderTaskProductId.required";

    public Guid Value { get; init; }

    public WorkOrderTaskProductId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}
