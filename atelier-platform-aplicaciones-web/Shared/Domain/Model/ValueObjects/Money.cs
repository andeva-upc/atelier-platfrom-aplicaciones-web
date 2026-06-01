using System;

namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

public record Money
{
    public static readonly Money Zero = new(0m);

    private const string NotNegativeMessageKey = "operations.error.money.cannotBeNegative";

    public decimal Amount { get; init; }

    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(NotNegativeMessageKey, nameof(amount));
        }

        Amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    public Money Plus(Money? other)
    {
        if (other == null) return this;
        return new Money(Amount + other.Amount);
    }

    public Money Minus(Money? other)
    {
        if (other == null) return this;
        return new Money(Amount - other.Amount);
    }

    public Money Multiply(int quantity)
    {
        return new Money(Amount * quantity);
    }

    public Money Multiply(decimal factor)
    {
        return new Money(Amount * factor);
    }

    public bool IsGreaterThan(Money? other)
    {
        return other != null && Amount > other.Amount;
    }

    public bool IsLessThan(Money? other)
    {
        return other != null && Amount < other.Amount;
    }
}