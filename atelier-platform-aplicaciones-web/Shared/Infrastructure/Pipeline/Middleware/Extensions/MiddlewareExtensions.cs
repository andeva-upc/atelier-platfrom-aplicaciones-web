namespace atelier_platform_aplicaciones_web.Shared.Infrastructure.Pipeline.Middleware.Extensions;

using atelier_platform_aplicaciones_web.Shared.Infrastructure.Pipeline.Middleware.Components;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
