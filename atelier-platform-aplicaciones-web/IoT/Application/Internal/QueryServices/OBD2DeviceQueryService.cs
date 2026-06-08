using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class OBD2DeviceQueryService : IOBD2DeviceQueryService
{
    private readonly IOBD2DeviceRepository _deviceRepository;

    public OBD2DeviceQueryService(IOBD2DeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<OBD2Device?> Handle(GetOBD2DeviceByIdQuery query)
    {
        return await _deviceRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<OBD2Device>> Handle(GetOBD2DevicesByBranchIdQuery query)
    {
        return await _deviceRepository.FindByBranchIdAsync(query.BranchId);
    }
}
