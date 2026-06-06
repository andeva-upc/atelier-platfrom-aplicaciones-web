namespace atelier_platform_aplicaciones_web.Shared.Domain.Model;

/// <summary>
///     Represents a domain error.
/// </summary>
/// <param name="Code">The unique error code (e.g. operations.error.task.notFound).</param>
/// <param name="Message">The fallback error message or Type.</param>
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
}
