using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record UpdateServiceCommand
{
    public UpdateServiceCommand(ServiceId serviceId, string name, Money price)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("core.error.name.required", nameof(name));
        if (price == null) throw new ArgumentException("core.error.price.required", nameof(price));

        ServiceId = serviceId;
        Name = name;
        Price = price;
    }

    public ServiceId ServiceId { get; init; }
    public string Name { get; init; }
    public Money Price { get; init; }
}
