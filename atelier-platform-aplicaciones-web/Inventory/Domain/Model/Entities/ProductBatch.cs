using System;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Entities;

public class ProductBatch
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public InventoryQuantity Quantity { get; private set; }
    public InventoryQuantity ReservedQuantity { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public int AvailableQuantity => Quantity.Value - ReservedQuantity.Value;

    protected ProductBatch()
    {
        Quantity = null!;
        ReservedQuantity = null!;
        Description = string.Empty;
    }

    public ProductBatch(Guid productId, int quantity, string description)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = new InventoryQuantity(quantity);
        ReservedQuantity = new InventoryQuantity(0);
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public void ReserveStock(int quantity)
    {
        if (quantity > AvailableQuantity)
            throw new InvalidOperationException("Not enough stock available in this batch.");

        ReservedQuantity = new InventoryQuantity(ReservedQuantity.Value + quantity);
    }
}
