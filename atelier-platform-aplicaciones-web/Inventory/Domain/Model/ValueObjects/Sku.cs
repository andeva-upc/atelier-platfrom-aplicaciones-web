namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;

public record Sku(string Value)
{
    public Sku() : this(string.Empty)
    {
    }
}
