namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class TelemetrySnapshot
{
    public Guid Id { get; private set; }
    public Guid Obd2DeviceRegistrationId { get; private set; }
    public Guid BranchId { get; private set; }
    public int Rpm { get; private set; }
    public int Temperature { get; private set; }
    public double? SpeedKmh { get; private set; }
    public int? OdometerKm { get; private set; }
    public double FuelLevelPercent { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Required by EF Core
    protected TelemetrySnapshot() {}

    public TelemetrySnapshot(Guid obd2DeviceRegistrationId, Guid branchId, 
        int? rpm, int? temperature, double? speedKmh, int? odometerKm, double? fuelLevelPercent)
    {
        Id = Guid.NewGuid();
        Obd2DeviceRegistrationId = obd2DeviceRegistrationId;
        BranchId = branchId;
        Rpm = rpm ?? 0;
        Temperature = temperature ?? 0;
        SpeedKmh = speedKmh;
        OdometerKm = odometerKm;
        FuelLevelPercent = fuelLevelPercent ?? 0.0;
        CreatedAt = DateTime.UtcNow;
    }
}
