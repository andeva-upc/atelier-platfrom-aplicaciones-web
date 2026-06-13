namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record Obd2RegistrationStatus(string Value)
{
    public static readonly Obd2RegistrationStatus Active = new("ACTIVE");
    public static readonly Obd2RegistrationStatus Inactive = new("INACTIVE");
}
