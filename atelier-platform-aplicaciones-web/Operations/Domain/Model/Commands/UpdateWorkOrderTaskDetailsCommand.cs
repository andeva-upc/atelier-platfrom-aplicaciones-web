using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record UpdateWorkOrderTaskDetailsCommand(
    WorkOrderId WorkOrderId, 
    WorkOrderTaskId TaskId, 
    ServiceId ServiceId, 
    MechanicId MechanicId, 
    TaskDescription Description);