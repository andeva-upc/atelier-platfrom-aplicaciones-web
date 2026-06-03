using System;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;

public record UpdateProductCommand(
    Guid ProductId,
    string Category,
    string Name,
    string Sku,
    string Description,
    decimal SalePrice,
    int MinimumStock
);
