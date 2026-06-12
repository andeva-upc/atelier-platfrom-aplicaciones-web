using System;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;

public record InventoryQuantity
{
    public int Value { get; init; }

    public InventoryQuantity(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Inventory quantity cannot be negative.");
        }
        Value = value;
    }

    public InventoryQuantity() : this(0)
    {
    }

    public InventoryQuantity Add(int quantity)
    {
        return new InventoryQuantity(Value + quantity);
    }

    public InventoryQuantity Subtract(int quantity)
    {
        if (Value - quantity < 0)
        {
            throw new InvalidOperationException("Insufficient inventory quantity.");
        }
        return new InventoryQuantity(Value - quantity);
    }
}
