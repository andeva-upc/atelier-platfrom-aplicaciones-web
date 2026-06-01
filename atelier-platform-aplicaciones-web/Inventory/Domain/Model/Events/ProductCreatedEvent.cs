using System;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Events;

public record ProductCreatedEvent(Guid ProductId, Guid BranchId) : IEvent;
