using System;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid VoucherId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal Amount { get; private set; }
    public string Method { get; private set; }
    public string Currency { get; private set; }
    public DateTimeOffset PaidAt { get; private set; }

    protected Payment()
    {
        Method = null!;
        Currency = null!;
    }

    public Payment(Guid voucherId, Guid branchId, decimal amount, string method, string currency)
    {
        Id = Guid.NewGuid();
        VoucherId = voucherId;
        BranchId = branchId;
        Amount = amount;
        Method = method;
        Currency = currency;
        PaidAt = DateTimeOffset.UtcNow;
    }
}
