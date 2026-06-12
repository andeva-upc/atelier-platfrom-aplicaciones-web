using System;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record CreateServiceResource(Guid BranchId, string Name, decimal Price);
