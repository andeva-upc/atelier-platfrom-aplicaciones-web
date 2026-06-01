namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

public record CustomerId
{
    private const string NotNullUuidMessage = "shared.error.customerId.required";
    public Guid Value { get; init; }
    public CustomerId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }
        Value = value;
    }
}
