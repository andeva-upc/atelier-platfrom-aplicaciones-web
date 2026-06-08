namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record OBD2DeviceResource(Guid Id, Guid BranchId, string MacAddress, string Status, DateTime? LastPing);
