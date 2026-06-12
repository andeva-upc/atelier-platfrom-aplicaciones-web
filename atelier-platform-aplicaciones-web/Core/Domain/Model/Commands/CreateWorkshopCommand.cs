using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record CreateWorkshopCommand
{
    public CreateWorkshopCommand(OwnerId ownerId, string businessName, string brandName, TaxId taxId, int mileageIntervalConfig)
    {
        if (string.IsNullOrWhiteSpace(businessName)) throw new ArgumentException("core.error.businessName.required");
        if (string.IsNullOrWhiteSpace(brandName)) throw new ArgumentException("core.error.brandName.required");

        OwnerId = ownerId;
        BusinessName = businessName;
        BrandName = brandName;
        TaxId = taxId;
        MileageIntervalConfig = mileageIntervalConfig;
    }

    public OwnerId OwnerId { get; init; }
    public string BusinessName { get; init; }
    public string BrandName { get; init; }
    public TaxId TaxId { get; init; }
    public int MileageIntervalConfig { get; init; }
}
