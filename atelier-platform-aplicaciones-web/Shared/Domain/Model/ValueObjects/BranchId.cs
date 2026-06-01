namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

public record BranchId
{
    private const string NotNullUuidMessage = "shared.error.branchId.required";
    public Guid Value { get; init; }
    public BranchId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(NotNullUuidMessage, nameof(value));
        }
        Value = value;
    }
}
