using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;

public partial class Branch : IUserAuditableEntity
{
    public Branch()
    {
        Id = null!;
        WorkshopId = null!;
        Code = string.Empty;
        Name = string.Empty;
        Address = null!;
        Phone = null!;
    }

    public Branch(WorkshopId workshopId, string code, string name, Address address, Phone phone) : this()
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("core.error.code.required");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("core.error.name.required");

        Id = new BranchId(Guid.NewGuid());
        WorkshopId = workshopId;
        Code = code;
        Name = name;
        Address = address;
        Phone = phone;
    }

    public Branch(BranchId id, WorkshopId workshopId, string code, string name, Address address, Phone phone) 
        : this(workshopId, code, name, address, phone)
    {
        Id = id;
    }

    public BranchId Id { get; private set; }
    public WorkshopId WorkshopId { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public Phone Phone { get; private set; }

    public void Update(string code, string name, Address address, Phone phone)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("core.error.code.required");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("core.error.name.required");

        Code = code;
        Name = name;
        Address = address;
        Phone = phone;
    }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }
}
