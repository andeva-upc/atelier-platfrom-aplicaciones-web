using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class ReportDtcErrorCommandFromResourceAssembler
{
    public static ReportDtcErrorCommand ToCommandFromResource(ReportDtcErrorResource resource)
    {
        return new ReportDtcErrorCommand(resource.DeviceId, resource.DtcCode, resource.Description);
    }
}
