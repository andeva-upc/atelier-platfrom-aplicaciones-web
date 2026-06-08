namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record ReportDtcErrorResource(
    Guid DeviceId,
    string DtcCode,
    string Description
);
