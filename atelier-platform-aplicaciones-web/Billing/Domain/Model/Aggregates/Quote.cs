using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;

public class Quote : IUserAuditableEntity
{
    public Guid Id { get; private set; }
    public Guid WorkOrderId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal SubtotalAmount { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public decimal TotalAmount { get; private set; }
    public QuoteStatus Status { get; private set; }

    // IAuditableEntity properties
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // IUserAuditableEntity properties
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Additional fields needed by the schema
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }

    protected Quote() { }

    public Quote(Guid workOrderId, Guid branchId, decimal subtotalAmount, decimal discountPercentage)
    {
        Id = Guid.NewGuid();
        WorkOrderId = workOrderId;
        BranchId = branchId;
        SubtotalAmount = subtotalAmount;
        DiscountPercentage = discountPercentage;
        Status = QuoteStatus.DRAFT;
        CreatedBy = Guid.Empty;
        
        CalculateFinalTotal();
    }

    public void ApplyDiscount(decimal discountPercentage)
    {
        DiscountPercentage = discountPercentage;
        CalculateFinalTotal();
    }

    public void Update(decimal subtotalAmount, decimal discountPercentage)
    {
        SubtotalAmount = subtotalAmount;
        DiscountPercentage = discountPercentage;
        CalculateFinalTotal();
    }

    public void Approve()
    {
        Status = QuoteStatus.APPROVED;
    }

    public void Cancel()
    {
        Status = QuoteStatus.CANCELLED;
    }

    private void CalculateFinalTotal()
    {
        var discountMultiplier = 1m - (DiscountPercentage / 100m);
        TotalAmount = SubtotalAmount * discountMultiplier;
    }
}
