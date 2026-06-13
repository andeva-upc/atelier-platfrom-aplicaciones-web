using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record LinkObd2DeviceToVehicleCommand(Obd2DeviceId Obd2DeviceId, BranchId BranchId, VehicleId VehicleId);
