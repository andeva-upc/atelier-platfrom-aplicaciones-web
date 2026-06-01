using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record ReopenTaskCommand(Guid WorkOrderId, Guid TaskId);