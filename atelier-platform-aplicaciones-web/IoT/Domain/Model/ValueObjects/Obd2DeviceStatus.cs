namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

public record Obd2DeviceStatus(string Value)
{
    public static readonly Obd2DeviceStatus Available = new("AVAILABLE");
    public static readonly Obd2DeviceStatus Linked = new("LINKED");
    public static readonly Obd2DeviceStatus NotAvailable = new("NOT_AVAILABLE");
}
