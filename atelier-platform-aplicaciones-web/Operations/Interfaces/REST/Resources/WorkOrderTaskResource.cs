using System;
using System.Collections.Generic;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record WorkOrderTaskResource(
    Guid Id, 
    Guid ServiceId, 
    Guid BranchId, 
    Guid AssignedMechanicId, 
    string Status, 
    string Description, 
    decimal Price, 
    DateTimeOffset? StartedAt, 
    DateTimeOffset? CompletedAt, 
    IEnumerable<WorkOrderTaskProductResource> Products,
    DateTimeOffset? CreatedAt);