using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities;

public class QuoteItem
{
    public Guid Id { get; private set; }
    public Guid QuoteId { get; private set; }
    public string Type { get; private set; } // PRODUCT or SERVICE
    public Guid ReferenceId { get; private set; }
    public string Description { get; private set; }
    public decimal Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money TotalPrice { get; private set; }

    protected QuoteItem() { }

    public QuoteItem(Guid quoteId, string type, Guid referenceId, string description, decimal quantity, Money unitPrice)
    {
        Id = Guid.NewGuid();
        QuoteId = quoteId;
        Type = type;
        ReferenceId = referenceId;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = CalculateTotal();
    }

    public Money CalculateTotal()
    {
        return UnitPrice.Multiply(Quantity);
    }
}
