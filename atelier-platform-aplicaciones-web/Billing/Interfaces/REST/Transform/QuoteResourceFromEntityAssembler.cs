using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;

public static class QuoteResourceFromEntityAssembler
{
    public static QuoteResource ToResourceFromEntity(Quote entity)
    {
        return new QuoteResource(
            entity.Id,
            entity.WorkOrderId,
            entity.BranchId,
            entity.SubtotalAmount,
            entity.DiscountPercentage,
            entity.TotalAmount,
            entity.Status.ToString()
        );
    }
}
