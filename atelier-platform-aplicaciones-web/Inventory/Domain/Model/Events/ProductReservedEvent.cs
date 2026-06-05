using System;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Events;

public record ProductReservedEvent(Guid ProductId, int Quantity, Guid BranchId) : atelier_platform_aplicaciones_web.Shared.Domain.Model.Events.IEvent;
