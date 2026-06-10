namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;

public record TaxRate(decimal Percentage)
{
    public TaxRate() : this(0.18m) { } // Default IGV in Peru

    public static TaxRate Of(decimal percentage) => new TaxRate(percentage);

    public Money CalculateTax(Money baseAmount)
    {
        return new Money(baseAmount.Amount * Percentage, baseAmount.Currency);
    }
}
