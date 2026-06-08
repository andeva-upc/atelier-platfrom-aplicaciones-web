using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;

public partial class Employee : IAuditableEntity
{
    public Employee()
    {
        Id = null!;
        UserId = null!;
        Name = null!;
        Document = null!;
        Phone = null!;
    }

    public Employee(UserId userId, PersonName name, Document document, Phone phone) : this()
    {
        Id = new EmployeeId(Guid.NewGuid());
        UserId = userId;
        Name = name;
        Document = document;
        Phone = phone;
    }

    public Employee(EmployeeId id, UserId userId, PersonName name, Document document, Phone phone) 
        : this(userId, name, document, phone)
    {
        Id = id;
    }

    public EmployeeId Id { get; private set; }
    public UserId UserId { get; private set; }
    public PersonName Name { get; private set; }
    public Document Document { get; private set; }
    public Phone Phone { get; private set; }

    public void Update(PersonName name, Document document, Phone phone)
    {
        Name = name;
        Document = document;
        Phone = phone;
    }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
