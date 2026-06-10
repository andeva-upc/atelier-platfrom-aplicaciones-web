namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public Money() : this(0m, "PEN") { }
    
    public static Money Of(decimal amount) => new Money(amount, "PEN");

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies.");
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(decimal factor) => new Money(Amount * factor, Currency);
}
