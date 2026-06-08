using System;
using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

public record AddTaskResource(
    [Required(ErrorMessage = "operations.error.resource.serviceId.required")] Guid ServiceId, 
    [Required(ErrorMessage = "operations.error.resource.mechanicId.required")] Guid MechanicId, 
    [Required(ErrorMessage = "operations.error.resource.description.required")] string Description, 
    [Required(ErrorMessage = "operations.error.resource.laborPrice.required")] decimal LaborPrice);