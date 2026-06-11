using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;

public partial class Customer : IAuditableEntity
{
    public Customer()
    {
        Id = null!;
        UserId = null!;
        BusinessName = string.Empty;
        Name = null!;
        Document = null!;
        Phone = null!;
    }

    public Customer(UserId userId, bool isCorporate, PersonName? name, string? businessName, Document document, Phone phone) : this()
    {
        if (isCorporate && string.IsNullOrWhiteSpace(businessName))
        {
            throw new ArgumentException("core.error.businessName.required");
        }
        if (!isCorporate && name == null)
        {
            throw new ArgumentException("core.error.personName.required");
        }

        Id = new CustomerId(Guid.NewGuid());
        UserId = userId;
        IsCorporate = isCorporate;
        Name = name;
        BusinessName = businessName ?? string.Empty;
        Document = document;
        Phone = phone;
    }

    public Customer(CustomerId id, UserId userId, bool isCorporate, PersonName? name, string? businessName, Document document, Phone phone)
        : this(userId, isCorporate, name, businessName, document, phone)
    {
        Id = id;
    }

    public CustomerId Id { get; private set; }
    public UserId UserId { get; private set; }
    public bool IsCorporate { get; private set; }
    public PersonName? Name { get; private set; }
    public string? BusinessName { get; private set; }
    public Document Document { get; private set; }
    public Phone Phone { get; private set; }

    public void Update(PersonName? name, string? businessName, Document document, Phone phone)
    {
        if (IsCorporate)
        {
            if (!Document.DocumentType.ToString().Equals(document.DocumentType.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("core.error.customer.corporateDocumentTypeImmutable");
            }
            if (string.IsNullOrWhiteSpace(businessName))
            {
                throw new ArgumentException("core.error.businessName.required");
            }
            BusinessName = businessName;
        }
        else
        {
            if (name == null)
            {
                throw new ArgumentException("core.error.personName.required");
            }
            Name = name;
        }

        Document = document;
        Phone = phone;
    }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
