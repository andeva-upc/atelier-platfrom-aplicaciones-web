namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record ReportDtcErrorCommand(
    Guid DeviceId,
    string DtcCode,
    string Description
);
