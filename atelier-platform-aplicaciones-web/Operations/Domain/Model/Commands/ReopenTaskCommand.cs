using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record ReopenTaskCommand(WorkOrderId WorkOrderId, WorkOrderTaskId TaskId);