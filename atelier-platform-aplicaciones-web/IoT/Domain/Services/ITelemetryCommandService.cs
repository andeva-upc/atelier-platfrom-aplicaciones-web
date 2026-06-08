using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Services;

public interface ITelemetryCommandService
{
    Task Handle(IngestTelemetryBatchCommand command);
}
