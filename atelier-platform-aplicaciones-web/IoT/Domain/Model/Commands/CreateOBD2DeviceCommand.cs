namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record CreateOBD2DeviceCommand(Guid BranchId, string MacAddress);
