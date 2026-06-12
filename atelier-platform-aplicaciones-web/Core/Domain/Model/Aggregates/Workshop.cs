using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;

public partial class Workshop : IAuditableEntity
{
    public Workshop()
    {
        Id = null!;
        OwnerId = null!;
        BusinessName = string.Empty;
        BrandName = string.Empty;
        TaxId = null!;
        MileageIntervalConfig = 1;
    }

    public Workshop(OwnerId ownerId, string businessName, string brandName, TaxId taxId, int mileageIntervalConfig) : this()
    {
        if (string.IsNullOrWhiteSpace(businessName)) throw new ArgumentException("core.error.businessName.required");
        if (string.IsNullOrWhiteSpace(brandName)) throw new ArgumentException("core.error.brandName.required");

        Id = new WorkshopId(Guid.NewGuid());
        OwnerId = ownerId;
        BusinessName = businessName;
        BrandName = brandName;
        TaxId = taxId;
        MileageIntervalConfig = mileageIntervalConfig;
    }

    public Workshop(WorkshopId id, OwnerId ownerId, string businessName, string brandName, TaxId taxId, int mileageIntervalConfig)
        : this(ownerId, businessName, brandName, taxId, mileageIntervalConfig)
    {
        Id = id;
    }

    public void Update(string businessName, string brandName, TaxId taxId, int mileageIntervalConfig)
    {
        if (string.IsNullOrWhiteSpace(businessName)) throw new ArgumentException("core.error.businessName.required");
        if (string.IsNullOrWhiteSpace(brandName)) throw new ArgumentException("core.error.brandName.required");

        BusinessName = businessName;
        BrandName = brandName;
        TaxId = taxId;
        MileageIntervalConfig = mileageIntervalConfig;
    }

    public WorkshopId Id { get; private set; }
    public OwnerId OwnerId { get; private set; }
    public string BusinessName { get; private set; }
    public string BrandName { get; private set; }
    public TaxId TaxId { get; private set; }
    public int MileageIntervalConfig { get; private set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }
}
