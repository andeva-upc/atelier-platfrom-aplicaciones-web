using System;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

public record ProductResource(
    Guid Id,
    Guid BranchId,
    string Category,
    string Name,
    string Sku,
    string Description,
    decimal SalePrice,
    int MinimumStock,
    int CurrentStock
);
