using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record AddTaskToWorkOrderCommand(
    Guid WorkOrderId, 
    ServiceId ServiceId, 
    MechanicId MechanicId, 
    TaskDescription Description, 
    Money LaborPrice);