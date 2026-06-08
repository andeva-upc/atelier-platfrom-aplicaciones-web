namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record AppointmentId
{
    private const string NotNullUuidMessage = "operations.error.appointmentId.required";

    public Guid Value { get; init; }

    public AppointmentId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }
            
        Value = value;
    }
}
