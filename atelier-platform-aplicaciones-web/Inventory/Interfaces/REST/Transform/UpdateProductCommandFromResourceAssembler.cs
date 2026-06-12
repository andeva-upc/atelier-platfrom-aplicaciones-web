using System;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Transform;

public static class UpdateProductCommandFromResourceAssembler
{
    public static UpdateProductCommand ToCommandFromResource(Guid productId, UpdateProductResource resource)
    {
        return new UpdateProductCommand(
            productId,
            resource.Category,
            resource.Name,
            resource.Sku,
            resource.Description,
            resource.SalePrice,
            resource.MinimumStock
        );
    }
}
