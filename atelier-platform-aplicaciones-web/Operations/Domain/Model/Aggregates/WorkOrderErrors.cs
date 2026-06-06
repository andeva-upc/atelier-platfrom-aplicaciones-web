using atelier_platform_aplicaciones_web.Shared.Domain.Model;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;

public static class WorkOrderErrors
{
    // Errores estáticos por defecto (basados en los de Java)
    public static readonly Error NotFound = new("operations.error.workOrder.notFound", "NotFound");
    public static readonly Error Duplicate = new("operations.error.workOrder.alreadyExistsForAppointment", "Duplicate");
    public static readonly Error UnexpectedError = new("UnexpectedError", "UnexpectedError");

    // Método para errores dinámicos de estado inválido
    public static Error InvalidState(string code) => new(code, "InvalidState");
    
    // Método para errores Not Found genéricos
    public static Error GenericNotFound(string code) => new(code, "NotFound");
}
