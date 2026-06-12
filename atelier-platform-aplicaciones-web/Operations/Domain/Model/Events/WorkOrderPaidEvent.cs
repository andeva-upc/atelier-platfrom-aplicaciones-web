using System;
using System.Collections.Generic;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Events;

public record WorkOrderPaidEvent(WorkOrderId WorkOrderId, BranchId BranchId, List<WorkOrderTaskProduct> DispatchedProducts) : IEvent;