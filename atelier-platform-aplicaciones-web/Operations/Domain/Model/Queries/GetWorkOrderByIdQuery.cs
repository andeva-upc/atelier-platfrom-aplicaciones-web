using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;

public record GetWorkOrderByIdQuery(WorkOrderId Id);