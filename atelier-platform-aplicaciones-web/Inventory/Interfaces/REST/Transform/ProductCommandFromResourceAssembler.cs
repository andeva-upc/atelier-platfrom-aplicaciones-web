using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Transform;

public static class ProductCommandFromResourceAssembler
{
    public static CreateProductCommand ToCommandFromResource(CreateProductResource resource)
    {
        return new CreateProductCommand(
            resource.BranchId,
            resource.Category,
            resource.Name,
            resource.Sku,
            resource.Description,
            resource.SalePrice,
            resource.MinimumStock
        );
    }
}
