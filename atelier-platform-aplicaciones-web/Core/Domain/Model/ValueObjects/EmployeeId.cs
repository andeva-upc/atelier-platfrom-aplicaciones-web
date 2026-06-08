using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record EmployeeId
{
    public EmployeeId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("core.error.employeeId.required");
        }
        Value = value;
    }

    public Guid Value { get; init; }
}
