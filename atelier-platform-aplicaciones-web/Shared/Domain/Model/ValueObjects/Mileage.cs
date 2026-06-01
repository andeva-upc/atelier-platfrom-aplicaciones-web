namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

public record Mileage
{
    private const string NotNullMessageKey = "operations.error.mileage.required";
    private const string NotNegativeMessageKey = "operations.error.mileage.cannotBeNegative";

    public int Value { get; init; }

    public Mileage(int? value)
    {
        if (!value.HasValue)
        {
            throw new ArgumentException(NotNullMessageKey, nameof(value));
        }

        if (value.Value < 0)
        {
            throw new ArgumentException(NotNegativeMessageKey, nameof(value));
        }

        Value = value.Value;
    }
}
