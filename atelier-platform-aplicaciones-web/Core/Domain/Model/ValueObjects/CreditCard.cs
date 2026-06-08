using System;
using System.Text.RegularExpressions;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record CreditCard
{
    public CreditCard()
    {
        CardNumber = string.Empty;
        CardHolderName = string.Empty;
        ExpirationDate = string.Empty;
        Cvv = string.Empty;
    }

    public CreditCard(string cardNumber, string cardHolderName, string expirationDate, string cvv)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || !Regex.IsMatch(cardNumber, @"^\d{16}$"))
        {
            throw new ArgumentException("core.error.cardNumber.invalid");
        }
        if (string.IsNullOrWhiteSpace(cardHolderName))
        {
            throw new ArgumentException("core.error.cardHolderName.required");
        }
        if (string.IsNullOrWhiteSpace(expirationDate) || !Regex.IsMatch(expirationDate, @"^(0[1-9]|1[0-2])/\d{2}$"))
        {
            throw new ArgumentException("core.error.expirationDate.invalid");
        }
        if (string.IsNullOrWhiteSpace(cvv) || !Regex.IsMatch(cvv, @"^\d{3}$"))
        {
            throw new ArgumentException("core.error.cvv.invalid");
        }

        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpirationDate = expirationDate;
        Cvv = cvv;
    }

    public string CardNumber { get; init; }
    public string CardHolderName { get; init; }
    public string ExpirationDate { get; init; }
    public string Cvv { get; init; }
}
