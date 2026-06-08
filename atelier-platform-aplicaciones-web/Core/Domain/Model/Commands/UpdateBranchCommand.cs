using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record UpdateBranchCommand
{
    public UpdateBranchCommand(BranchId id, string code, string name, Address address, Phone phone)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("core.error.code.required");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("core.error.name.required");

        Id = id;
        Code = code;
        Name = name;
        Address = address;
        Phone = phone;
    }

    public BranchId Id { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
    public Address Address { get; init; }
    public Phone Phone { get; init; }
}
