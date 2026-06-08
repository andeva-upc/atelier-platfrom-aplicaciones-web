namespace atelier_platform_aplicaciones_web.Core.Domain.Model;

public enum CoreError
{
    None,
    WorkshopNotFound,
    BranchCodeMustBeUnique,
    BranchNotFound,
    CustomerProfileAlreadyExists,
    CustomerNotFound,
    EmployeeProfileAlreadyExists,
    EmployeeNotFound,
    OwnerProfileAlreadyExists,
    OwnerNotFound,
    SubscriptionPlanNotFound,
    BranchNoActiveSubscription
}
