using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;

public record CreateWorkOrderCommand(
    AppointmentId AppointmentId, 
    BranchId BranchId, 
    VehicleId VehicleId, 
    CustomerId CustomerId,
    DiagnosticSummary DiagnosticSummary, 
    Mileage MileageIn);