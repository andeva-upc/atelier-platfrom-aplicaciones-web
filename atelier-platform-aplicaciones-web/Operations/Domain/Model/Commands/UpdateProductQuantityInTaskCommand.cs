using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record UpdateProductQuantityInTaskCommand(
    Guid WorkOrderId, 
    Guid TaskId, 
    ProductId ProductId, 
    Quantity NewQuantity);