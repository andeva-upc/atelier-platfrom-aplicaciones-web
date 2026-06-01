namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;

public record ProductName(string Name)
{
    public ProductName() : this(string.Empty)
    {
    }
}
