using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record CreateWorkshopCommand
{
    public CreateWorkshopCommand(OwnerId ownerId, string businessName, string brandName, string taxId, int mileageIntervalConfig)
    {
        if (string.IsNullOrWhiteSpace(businessName)) throw new ArgumentException("core.error.businessName.required");
        if (string.IsNullOrWhiteSpace(brandName)) throw new ArgumentException("core.error.brandName.required");
        if (string.IsNullOrWhiteSpace(taxId)) throw new ArgumentException("core.error.taxId.required");

        OwnerId = ownerId;
        BusinessName = businessName;
        BrandName = brandName;
        TaxId = taxId;
        MileageIntervalConfig = mileageIntervalConfig;
    }

    public OwnerId OwnerId { get; init; }
    public string BusinessName { get; init; }
    public string BrandName { get; init; }
    public string TaxId { get; init; }
    public int MileageIntervalConfig { get; init; }
}
