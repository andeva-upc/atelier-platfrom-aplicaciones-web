using MediatR;
using Microsoft.Extensions.Logging;

namespace atelier_platform_aplicaciones_web.Shared.Infrastructure.Mediator.Cortex.Configuration;

/// <summary>
///     Logging command behavior
/// </summary>
/// <remarks>
///     This behavior is used to log the execution of a command.
///     It logs the start and the end of the command execution.
/// </remarks>
/// <typeparam name="TRequest">The command type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public class LoggingCommandBehavior<TRequest, TResponse>(ILogger<LoggingCommandBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;
        logger.LogInformation("Handling {RequestName}", requestName);
        var response = await next();
        logger.LogInformation("Handled {RequestName}", requestName);
        return response;
    }
}
