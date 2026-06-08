namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record LinkDeviceResource(Guid Obd2DeviceId, Guid VehicleId, Guid BranchId);
