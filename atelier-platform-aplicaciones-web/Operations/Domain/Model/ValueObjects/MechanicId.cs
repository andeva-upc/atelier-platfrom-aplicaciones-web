namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record MechanicId
{
    private const string NotNullUuidMessage = "operations.error.mechanicId.required";

    public Guid Value { get; init; }

    public MechanicId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }
            
        Value = value;
    }
}
