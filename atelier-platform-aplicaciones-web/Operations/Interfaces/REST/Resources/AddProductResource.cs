using System;
using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record AddProductResource(
    [Required(ErrorMessage = "operations.error.resource.productId.required")] Guid ProductId, 
    [Required(ErrorMessage = "operations.error.resource.quantity.required")] int Quantity, 
    [Required(ErrorMessage = "operations.error.resource.unitPrice.required")] decimal UnitPrice);