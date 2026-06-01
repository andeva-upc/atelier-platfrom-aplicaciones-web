using System;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record WorkOrderTaskProductResource(
    Guid Id, 
    Guid ProductId, 
    Guid BranchId, 
    int Quantity, 
    decimal UnitPrice, 
    decimal TotalAmount,
    DateTimeOffset? CreatedAt);