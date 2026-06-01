using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Events;

public record ProductReservedEvent(Guid WorkOrderId, BranchId BranchId, ProductId ProductId, Quantity Quantity);