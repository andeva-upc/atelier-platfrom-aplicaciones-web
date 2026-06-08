using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record Phone
{
    public Phone()
    {
        Value = string.Empty;
    }

    public Phone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("core.error.phone.required");
        }
        Value = value;
    }

    public string Value { get; init; }
}
