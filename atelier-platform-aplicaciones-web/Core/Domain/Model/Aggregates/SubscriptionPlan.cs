using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;

public partial class SubscriptionPlan
{
    public SubscriptionPlan()
    {
        Id = null!;
        Name = string.Empty;
        IsActive = true;
    }

    public SubscriptionPlan(string name, double monthlyPrice, int maxObd2Devices, int maxMonthlySnapshotsPerVehicle, int maxCustomers, int maxStaffAccounts, bool isActive) : this()
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("core.error.name.required");

        Id = new SubscriptionPlanId(Guid.NewGuid());
        Name = name;
        MonthlyPrice = monthlyPrice;
        MaxObd2Devices = maxObd2Devices;
        MaxMonthlySnapshotsPerVehicle = maxMonthlySnapshotsPerVehicle;
        MaxCustomers = maxCustomers;
        MaxStaffAccounts = maxStaffAccounts;
        IsActive = isActive;
    }

    public SubscriptionPlan(SubscriptionPlanId id, string name, double monthlyPrice, int maxObd2Devices, int maxMonthlySnapshotsPerVehicle, int maxCustomers, int maxStaffAccounts, bool isActive)
        : this(name, monthlyPrice, maxObd2Devices, maxMonthlySnapshotsPerVehicle, maxCustomers, maxStaffAccounts, isActive)
    {
        Id = id;
    }

    public SubscriptionPlanId Id { get; private set; }
    public string Name { get; private set; }
    public double MonthlyPrice { get; private set; }
    public int MaxObd2Devices { get; private set; }
    public int MaxMonthlySnapshotsPerVehicle { get; private set; }
    public int MaxCustomers { get; private set; }
    public int MaxStaffAccounts { get; private set; }
    public bool IsActive { get; private set; }
}
