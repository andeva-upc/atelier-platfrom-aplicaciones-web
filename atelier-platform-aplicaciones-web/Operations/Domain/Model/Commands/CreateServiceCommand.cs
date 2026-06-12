using System;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record CreateServiceCommand
{
    public CreateServiceCommand(BranchId branchId, string name, Money price)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("core.error.name.required", nameof(name));
        if (price == null) throw new ArgumentException("core.error.price.required", nameof(price));

        BranchId = branchId;
        Name = name;
        Price = price;
    }

    public BranchId BranchId { get; init; }
    public string Name { get; init; }
    public Money Price { get; init; }
}
