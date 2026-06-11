using System;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;

public class Voucher : IUserAuditableEntity
{
    public Guid Id { get; private set; }
    public Guid QuoteId { get; private set; }
    public Guid BranchId { get; private set; }
    public string Series { get; private set; }
    public int VoucherNumber { get; private set; }
    public decimal SubtotalAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Type { get; private set; }
    public string Status { get; private set; }
    public string Currency { get; private set; }

    // IAuditableEntity properties
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // IUserAuditableEntity properties
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Additional fields needed by the schema
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }

    protected Voucher() { }

    public Voucher(Guid quoteId, Guid branchId, string series, int voucherNumber, decimal subtotalAmount, string type, string currency)
    {
        Id = Guid.NewGuid();
        QuoteId = quoteId;
        BranchId = branchId;
        Series = series;
        VoucherNumber = voucherNumber;
        SubtotalAmount = subtotalAmount;
        Type = type;
        Currency = currency;
        Status = VoucherStatus.PENDING;
        CreatedBy = Guid.Empty;
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        // Total is Subtotal + 18% IGV/Tax
        TotalAmount = SubtotalAmount * 1.18m;
    }

    public void MarkAsPaid()
    {
        Status = VoucherStatus.PAID;
    }
}
