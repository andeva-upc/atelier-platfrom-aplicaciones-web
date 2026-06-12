using System;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

public record ProductBatchResource(Guid BatchId, int AvailableQuantity, int ReservedQuantity, decimal AcquisitionCost, DateTime CreatedAt);
