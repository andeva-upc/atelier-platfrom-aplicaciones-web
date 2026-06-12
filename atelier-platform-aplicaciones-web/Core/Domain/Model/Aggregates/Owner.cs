using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;

public partial class Owner : IAuditableEntity
{
    public Owner()
    {
        Id = null!;
        UserId = null!;
        Name = null!;
        Document = null!;
        Phone = null!;
    }

    public Owner(UserId userId, PersonName name, Document document, Phone phone) : this()
    {
        Id = new OwnerId(Guid.NewGuid());
        UserId = userId;
        Name = name;
        Document = document;
        Phone = phone;
    }

    public Owner(OwnerId id, UserId userId, PersonName name, Document document, Phone phone) 
        : this(userId, name, document, phone)
    {
        Id = id;
    }

    public OwnerId Id { get; private set; }
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
    public long Version { get; set; }
}
