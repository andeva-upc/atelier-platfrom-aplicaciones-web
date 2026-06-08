using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record UpdateProductQuantityInTaskResource(
    [Required(ErrorMessage = "operations.error.resource.newQuantity.required")] int NewQuantity);