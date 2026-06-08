using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Services;

public interface IOBD2DeviceQueryService
{
    Task<OBD2Device?> Handle(GetOBD2DeviceByIdQuery query);
    Task<IEnumerable<OBD2Device>> Handle(GetOBD2DevicesByBranchIdQuery query);
}
