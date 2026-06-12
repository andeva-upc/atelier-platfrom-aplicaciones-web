using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;

public class Service : IUserAuditableEntity
{
    public Service()
    {
        Id = null!;
        BranchId = null!;
        Name = string.Empty;
        Price = null!;
    }

    public Service(BranchId branchId, string name, Money price) : this()
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("core.error.name.required", nameof(name));
        if (price == null) throw new ArgumentException("core.error.price.required", nameof(price));

        Id = new ServiceId(Guid.NewGuid());
        BranchId = branchId;
        Name = name;
        Price = price;
    }

    public Service(ServiceId id, BranchId branchId, string name, Money price) : this(branchId, name, price)
    {
        Id = id;
    }

    public void Update(string name, Money price)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("core.error.name.required", nameof(name));
        if (price == null) throw new ArgumentException("core.error.price.required", nameof(price));

        Name = name;
        Price = price;
    }

    public ServiceId Id { get; private set; }
    public BranchId BranchId { get; private set; }
    public string Name { get; private set; }
    public Money Price { get; private set; }

    // Campos de Auditoría y Concurrencia Integrados
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public long Version { get; set; }
}
