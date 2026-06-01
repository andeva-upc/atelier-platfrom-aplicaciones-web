using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record ProductId
{
    private const string NotNullUuidMessage = "operations.error.productId.required";

    public Guid Value { get; init; }

    public ProductId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}