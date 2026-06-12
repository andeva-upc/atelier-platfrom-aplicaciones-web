using System;
using System.Text.RegularExpressions;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record TaxId
{
    private static readonly Regex TaxIdRegex = new(@"^\d{11}$", RegexOptions.Compiled);

    public TaxId(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !TaxIdRegex.IsMatch(value))
        {
            throw new ArgumentException("core.error.taxId.invalid");
        }
        Value = value;
    }

    public string Value { get; init; }
}
