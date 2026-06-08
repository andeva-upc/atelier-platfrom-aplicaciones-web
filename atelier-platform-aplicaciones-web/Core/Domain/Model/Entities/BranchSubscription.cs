using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Entities;

public class BranchSubscription
{
    public BranchSubscription()
    {
        Id = null!;
        BranchId = null!;
        PlanId = null!;
    }

    public BranchSubscription(BranchId branchId, SubscriptionPlanId planId, BillingCycle billingCycle, DateTime startDate, DateTime endDate) : this()
    {
        if (startDate == default) throw new ArgumentException("core.error.startDate.required");
        if (endDate == default) throw new ArgumentException("core.error.endDate.required");

        Id = new BranchSubscriptionId(Guid.NewGuid());
        BranchId = branchId;
        PlanId = planId;
        Status = SubscriptionStatus.Active;
        BillingCycle = billingCycle;
        StartDate = startDate;
        EndDate = endDate;
    }

    public BranchSubscription(BranchSubscriptionId id, BranchId branchId, SubscriptionPlanId planId, SubscriptionStatus status, BillingCycle billingCycle, DateTime startDate, DateTime endDate, DateTime? canceledAt)
        : this(branchId, planId, billingCycle, startDate, endDate)
    {
        Id = id;
        Status = status;
        CanceledAt = canceledAt;
    }

    public void Cancel(DateTime canceledAt)
    {
        Status = SubscriptionStatus.Canceled;
        CanceledAt = canceledAt;
    }

    public BranchSubscriptionId Id { get; private set; }
    public BranchId BranchId { get; private set; }
    public SubscriptionPlanId PlanId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public BillingCycle BillingCycle { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime? CanceledAt { get; private set; }
}
