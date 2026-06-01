using System;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

public record CreateProductResource(
    Guid BranchId,
    string Category,
    string Name,
    string Sku,
    string Description,
    decimal SalePrice,
    int MinimumStock
);
