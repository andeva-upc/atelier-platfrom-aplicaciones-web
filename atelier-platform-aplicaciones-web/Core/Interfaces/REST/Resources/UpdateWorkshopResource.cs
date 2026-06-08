namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record UpdateWorkshopResource(
    string BusinessName,
    string BrandName,
    string TaxId,
    int MileageIntervalConfig
);
