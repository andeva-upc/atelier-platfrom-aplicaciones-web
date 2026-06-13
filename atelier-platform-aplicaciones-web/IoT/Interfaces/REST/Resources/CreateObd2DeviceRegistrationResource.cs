using System;
using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record CreateObd2DeviceRegistrationResource(
    [Required] Guid Obd2DeviceId,
    [Required] Guid BranchId,
    [Required] Guid VehicleId
);
