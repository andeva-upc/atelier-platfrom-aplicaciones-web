using System;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;

public class Voucher : IUserAuditableEntity
{
    public Guid Id { get; private set; }
    public Guid QuoteId { get; private set; }
    public Guid BranchId { get; private set; }
    public int VoucherNumber { get; private set; }
    public decimal SubtotalAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Type { get; private set; }
    public string Status { get; private set; }
    public string Currency { get; private set; }
    
    public string? CustomerDocumentType { get; private set; }
    public string? CustomerDocumentNumber { get; private set; }
    public string? CustomerName { get; private set; }
    public Guid? ExternalInvoiceId { get; private set; }

    public ICollection<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment> Payments { get; private set; }

    // IAuditableEntity properties
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // IUserAuditableEntity properties
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Additional fields needed by the schema
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }

    protected Voucher()
    {
        Type = null!;
        Status = null!;
        Currency = null!;
        Payments = new System.Collections.Generic.List<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>();
    }

    public Voucher(Guid quoteId, Guid branchId, int voucherNumber, decimal subtotalAmount, string type, string currency, string? customerDocType, string? customerDocNum, string? customerName, Guid? externalInvoiceId)
    {
        Id = Guid.NewGuid();
        QuoteId = quoteId;
        BranchId = branchId;
        VoucherNumber = voucherNumber;
        SubtotalAmount = subtotalAmount;
        Type = type;
        Currency = currency;
        CustomerDocumentType = customerDocType;
        CustomerDocumentNumber = customerDocNum;
        CustomerName = customerName;
        ExternalInvoiceId = externalInvoiceId;
        Status = VoucherStatus.PENDING;
        CreatedBy = Guid.Empty;
        Payments = new System.Collections.Generic.List<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>();
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        // Total is Subtotal + 18% IGV/Tax
        TotalAmount = SubtotalAmount * 1.18m;
    }

    public void AddPayment(decimal amount, string method, string currency)
    {
        if (Status == VoucherStatus.PAID)
            throw new Exception("Voucher is already fully paid.");

        if (Status == VoucherStatus.CANCELED)
            throw new Exception("Cannot add payment to a canceled voucher.");

        var totalPaid = System.Linq.Enumerable.Sum(Payments, p => p.Amount);
        var remaining = TotalAmount - totalPaid;

        if (amount > remaining)
            throw new Exception("Payment amount exceeds the remaining balance.");

        var payment = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment(Id, BranchId, amount, method, currency);
        Payments.Add(payment);

        if (totalPaid + amount >= TotalAmount)
        {
            Status = VoucherStatus.PAID;
        }
    }

    public void MarkAsPaid()
    {
        Status = VoucherStatus.PAID;
    }
}
