namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record OBD2DeviceRegistrationResource(
    Guid Id,
    Guid Obd2DeviceId,
    Guid BranchId,
    Guid VehicleId,
    string Status,
    DateTime CreatedAt,
    DateTime? DeletedAt
);
