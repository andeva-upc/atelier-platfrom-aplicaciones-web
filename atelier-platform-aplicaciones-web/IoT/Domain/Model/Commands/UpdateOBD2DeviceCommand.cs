namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record UpdateOBD2DeviceCommand(Guid Id, Guid BranchId, string MacAddress);
