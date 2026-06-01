using System;
namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record CreateWorkOrderResource(
    Guid AppointmentId, 
    Guid BranchId, 
    Guid VehicleId, 
    Guid CustomerId, 
    string DiagnosticSummary, 
    int MileageIn);