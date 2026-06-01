namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record MechanicId
{
    public Guid Value { get; init; }

    public MechanicId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("operations.error.mechanicId.required", nameof(value));
            
        Value = value;
    }
}
