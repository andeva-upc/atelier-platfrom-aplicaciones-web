namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record AppointmentId
{
    public Guid Value { get; init; }

    public AppointmentId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("operations.error.appointmentId.required", nameof(value));
            
        Value = value;
    }
}
