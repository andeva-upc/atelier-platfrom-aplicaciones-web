using System;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

public enum WorkOrderTaskStatus
{
    Pending,
    Doing,
    Completed
}

public static class WorkOrderTaskStatusExtensions
{
    private const string InvalidTransitionMessage = "operations.error.workOrderTaskStatus.invalidTransition";

    public static bool CanTransitionTo(this WorkOrderTaskStatus current, WorkOrderTaskStatus next)
    {
        return current switch
        {
            WorkOrderTaskStatus.Pending => next == WorkOrderTaskStatus.Doing,
            WorkOrderTaskStatus.Doing => next == WorkOrderTaskStatus.Completed,
            WorkOrderTaskStatus.Completed => next == WorkOrderTaskStatus.Doing,
            _ => false
        };
    }

    public static WorkOrderTaskStatus TransitionTo(this WorkOrderTaskStatus current, WorkOrderTaskStatus next)
    {
        if (!current.CanTransitionTo(next))
        {
            throw new InvalidOperationException(InvalidTransitionMessage);
        }
        return next;
    }
}