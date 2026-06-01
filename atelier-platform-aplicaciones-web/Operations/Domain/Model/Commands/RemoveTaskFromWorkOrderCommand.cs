using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record RemoveTaskFromWorkOrderCommand(Guid WorkOrderId, Guid TaskId);