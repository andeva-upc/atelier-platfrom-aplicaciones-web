namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;

public record ProductCategory(string Value)
{
    public ProductCategory() : this(string.Empty)
    {
    }
}
