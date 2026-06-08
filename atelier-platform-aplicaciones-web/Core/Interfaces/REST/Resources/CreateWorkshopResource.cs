using System;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record CreateWorkshopResource(
    Guid OwnerId,
    string BusinessName,
    string BrandName,
    string TaxId,
    int MileageIntervalConfig
);
