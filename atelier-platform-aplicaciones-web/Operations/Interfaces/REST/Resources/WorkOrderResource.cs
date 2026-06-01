using System;
using System.Collections.Generic;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record WorkOrderResource(
    Guid Id, 
    Guid AppointmentId, 
    Guid BranchId, 
    Guid VehicleId, 
    Guid CustomerId, 
    int InternalNumber, 
    string FormattedNumber, 
    string Status, 
    string DiagnosticSummary, 
    int MileageIn, 
    decimal TotalAmount, 
    IEnumerable<WorkOrderTaskResource> Tasks,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? UpdatedAt);