using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public record TaskDescription
{
    private const int MinLength = 10;
    private const int MaxLength = 1000;
    private const string NotBlankKey = "operations.error.taskDescription.required";
    private const string TooShortKey = "operations.error.taskDescription.tooShort";
    private const string TooLongKey = "operations.error.taskDescription.tooLong";

    public string Value { get; init; }

    public TaskDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(NotBlankKey, nameof(value));
        }

        if (value.Trim().Length < MinLength)
        {
            throw new ArgumentException(TooShortKey, nameof(value));
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentException(TooLongKey, nameof(value));
        }

        Value = value;
    }
}