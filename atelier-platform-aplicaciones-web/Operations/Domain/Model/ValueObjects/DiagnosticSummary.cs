using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record DiagnosticSummary
{
    private const int MaxLength = 2000;
    private const string NotBlankMessageKey = "operations.error.diagnosticSummary.notBlank";
    private const string SizeMessageKey = "operations.error.diagnosticSummary.tooLong";

    public string Value { get; init; }

    public DiagnosticSummary(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(NotBlankMessageKey, nameof(value));
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentException(SizeMessageKey, nameof(value));
        }

        Value = value;
    }
}