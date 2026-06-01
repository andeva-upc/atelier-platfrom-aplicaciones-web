namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

public record Address
{
    private const int MaxLength = 100;
    private const string NotBlankMessageKey = "operations.error.address.notBlank";
    private const string SizeMessageKey = "operations.error.address.tooLong";
    public string Value { get; init; }
    public Address(string value)
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
