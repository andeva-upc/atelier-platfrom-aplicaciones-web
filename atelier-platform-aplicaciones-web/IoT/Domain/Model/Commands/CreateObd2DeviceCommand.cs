using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record CreateObd2DeviceCommand(BranchId BranchId, string MacAddress);
