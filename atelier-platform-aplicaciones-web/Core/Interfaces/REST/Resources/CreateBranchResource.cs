using System;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record CreateBranchResource(
    Guid WorkshopId,
    string Code,
    string Name,
    string Address,
    string Phone
);
