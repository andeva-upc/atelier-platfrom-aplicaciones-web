using System;
using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record CreateWorkOrderResource(
    [Required(ErrorMessage = "operations.error.resource.appointmentId.required")] Guid AppointmentId, 
    [Required(ErrorMessage = "operations.error.resource.branchId.required")] Guid BranchId, 
    [Required(ErrorMessage = "operations.error.resource.vehicleId.required")] Guid VehicleId, 
    [Required(ErrorMessage = "operations.error.resource.customerId.required")] Guid CustomerId, 
    [Required(ErrorMessage = "operations.error.resource.diagnosticSummary.required")] string DiagnosticSummary, 
    [Required(ErrorMessage = "operations.error.resource.mileageIn.required")] int MileageIn);