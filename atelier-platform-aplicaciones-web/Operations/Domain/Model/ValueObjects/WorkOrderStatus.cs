using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public enum WorkOrderStatus
{
    Pending,
    InProgress,
    Completed,
    Paid
}

public static class WorkOrderStatusExtensions
{
    private const string InvalidTransitionMessage = "operations.error.workOrderStatus.invalidTransition";

    public static bool CanTransitionTo(this WorkOrderStatus current, WorkOrderStatus next)
    {
        return current switch
        {
            WorkOrderStatus.Pending => next == WorkOrderStatus.InProgress,
            WorkOrderStatus.InProgress => next == WorkOrderStatus.Completed,
            WorkOrderStatus.Completed => next == WorkOrderStatus.Paid || next == WorkOrderStatus.InProgress,
            WorkOrderStatus.Paid => false,
            _ => false
        };
    }

    public static WorkOrderStatus TransitionTo(this WorkOrderStatus current, WorkOrderStatus next)
    {
        if (!current.CanTransitionTo(next))
        {
            throw new InvalidOperationException(InvalidTransitionMessage);
        }
        return next;
    }
}