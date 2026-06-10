using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;

public static class QuoteResourceFromEntityAssembler
{
    public static QuoteResource ToResourceFromEntity(Quote entity)
    {
        return new QuoteResource(
            entity.Id,
            entity.WorkshopId,
            entity.CustomerId,
            entity.VehicleId,
            entity.Description,
            entity.Currency,
            entity.Subtotal.Amount,
            entity.DiscountAmount.Amount,
            entity.TaxRate.Percentage,
            entity.Total.Amount,
            entity.Status.ToString()
        );
    }
}
