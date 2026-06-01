namespace atelier_platform_aplicaciones_web.Operations.Application.Errors;

public record WorkOrderError(string Code, string Type)
{
    // Los errores estáticos por defecto
    public static readonly WorkOrderError NotFound = new("operations.error.workOrder.notFound", "NotFound");
    public static readonly WorkOrderError Duplicate = new("operations.error.workOrder.alreadyExistsForAppointment", "Duplicate");
    public static readonly WorkOrderError UnexpectedError = new("UnexpectedError", "UnexpectedError");

    // Método para errores dinámicos de estado inválido
    public static WorkOrderError InvalidState(string code) => new(code, "InvalidState");
}