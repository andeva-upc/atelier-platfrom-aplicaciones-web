namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record TelemetrySnapshotResource(
    Guid Id,
    Guid Obd2DeviceRegistrationId,
    Guid BranchId,
    int Rpm,
    int Temperature,
    double? SpeedKmh,
    int? OdometerKm,
    double FuelLevelPercent,
    DateTime CreatedAt
);
