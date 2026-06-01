using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Transform;

public static class ProductResourceFromEntityAssembler
{
    public static ProductResource ToResourceFromEntity(Product entity)
    {
        return new ProductResource(
            entity.Id,
            entity.BranchId.Value,
            entity.Category.Value,
            entity.Name.Name,
            entity.Sku.Value,
            entity.Description,
            entity.CurrentSellingPrice.Amount,
            entity.MinimumStock,
            entity.CurrentStock.Value
        );
    }
}
