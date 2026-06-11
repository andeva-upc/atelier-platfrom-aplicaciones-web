using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;

public static class GenerateVoucherCommandFromResourceAssembler
{
    public static GenerateVoucherCommand ToCommandFromResource(GenerateVoucherResource resource)
    {
        return new GenerateVoucherCommand(
            resource.QuoteId,
            resource.Type,
            resource.CustomerDocumentType,
            resource.CustomerDocumentNumber,
            resource.CustomerName
        );
    }
}
