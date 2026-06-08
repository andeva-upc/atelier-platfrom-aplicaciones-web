using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record UpdateWorkOrderDetailsResource(
    [Required(ErrorMessage = "operations.error.resource.diagnosticSummary.required")] string DiagnosticSummary, 
    [Required(ErrorMessage = "operations.error.resource.mileageIn.required")] int MileageIn);