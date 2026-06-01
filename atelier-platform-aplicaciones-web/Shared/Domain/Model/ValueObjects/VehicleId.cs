namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

public record VehicleId
{
    private const string NotNullUuidMessage = "shared.error.vehicleId.required";

    public Guid Value { get; init; }

    public VehicleId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }

        Value = value;
    }
}