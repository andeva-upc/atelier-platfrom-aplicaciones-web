using System;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;

public record CreateProductCommand(
    Guid BranchId,
    string Category,
    string Name,
    string Sku,
    string Description,
    decimal SalePrice,
    int MinimumStock
);
