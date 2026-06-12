using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Events;

public record ProductReservationCanceledEvent(WorkOrderId WorkOrderId, BranchId BranchId, ProductId ProductId, Quantity Quantity) : IEvent;