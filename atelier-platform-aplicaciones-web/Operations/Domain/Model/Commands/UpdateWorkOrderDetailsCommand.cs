using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record UpdateWorkOrderDetailsCommand(
    WorkOrderId WorkOrderId, 
    DiagnosticSummary DiagnosticSummary, 
    Mileage MileageIn);