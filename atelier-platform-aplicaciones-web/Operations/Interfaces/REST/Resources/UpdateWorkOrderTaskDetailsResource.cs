using System;
namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record UpdateWorkOrderTaskDetailsResource(Guid ServiceId, Guid MechanicId, string Description, decimal LaborPrice);