using System;
using System.Collections.Generic;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Events;

public record WorkOrderPaidEvent(Guid WorkOrderId, BranchId BranchId, List<WorkOrderTaskProduct> DispatchedProducts) : IEvent;