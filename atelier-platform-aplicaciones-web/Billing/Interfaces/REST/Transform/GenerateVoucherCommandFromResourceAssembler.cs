using System.Linq;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;

public static class GenerateVoucherCommandFromResourceAssembler
{
    public static GenerateVoucherCommand ToCommandFromResource(GenerateVoucherResource resource)
    {
        var items = resource.Items.Select(i => new VoucherItemRecord(i.Description, i.Quantity, i.UnitPrice)).ToList();

        return new GenerateVoucherCommand(
            resource.QuoteId,
            resource.BranchId,
            resource.SubtotalAmount,
            resource.Type,
            resource.Currency,
            items
        );
    }
}
