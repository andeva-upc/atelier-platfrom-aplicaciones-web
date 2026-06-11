using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;

public static class VoucherResourceFromEntityAssembler
{
    public static VoucherResource ToResourceFromEntity(Voucher entity)
    {
        return new VoucherResource(
            entity.Id,
            entity.QuoteId,
            entity.BranchId,
            entity.VoucherNumber,
            entity.SubtotalAmount,
            entity.TotalAmount,
            entity.Type,
            entity.Status,
            entity.Currency
        );
    }
}
