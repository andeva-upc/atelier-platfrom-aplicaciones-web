using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;

public static class CreateQuoteCommandFromResourceAssembler
{
    public static CreateQuoteCommand ToCommandFromResource(CreateQuoteResource resource)
    {
        return new CreateQuoteCommand(
            resource.WorkOrderId,
            resource.BranchId,
            resource.SubtotalAmount,
            resource.DiscountPercentage
        );
    }
}
