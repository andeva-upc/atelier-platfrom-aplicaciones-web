using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;

public record GetWorkOrdersByBranchIdQuery(BranchId BranchId);