using System;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Queries;

public record GetProductsByBranchIdQuery(Guid BranchId);
