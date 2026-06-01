using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record Quantity
{
    private const string NotNullMessageKey = "operations.error.quantity.required";
    private const string StrictlyPositiveMessageKey = "operations.error.quantity.mustBeGreaterThanZero";

    public int Value { get; init; }

    public Quantity(int? value)
    {
        if (!value.HasValue)
        {
            throw new ArgumentException(NotNullMessageKey, nameof(value));
        }

        if (value.Value <= 0)
        {
            throw new ArgumentException(StrictlyPositiveMessageKey, nameof(value));
        }

        Value = value.Value;
    }
}