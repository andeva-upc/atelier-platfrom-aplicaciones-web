using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class TelemetryQueryService : ITelemetryQueryService
{
    private readonly IOBD2DeviceRegistrationRepository _registrationRepository;
    private readonly ITelemetrySnapshotRepository _snapshotRepository;

    public TelemetryQueryService(
        IOBD2DeviceRegistrationRepository registrationRepository,
        ITelemetrySnapshotRepository snapshotRepository)
    {
        _registrationRepository = registrationRepository;
        _snapshotRepository = snapshotRepository;
    }

    public async Task<TelemetrySnapshot?> Handle(GetLatestTelemetryByDeviceIdQuery query)
    {
        var activeReg = await _registrationRepository.FindByObd2DeviceIdAndStatusAsync(
            query.DeviceId, OBD2DeviceRegistrationStatus.ACTIVE);

        if (activeReg == null)
        {
            return null;
        }

        return await _snapshotRepository.FindLatestByRegistrationIdAsync(activeReg.Id);
    }

    public async Task<IEnumerable<TelemetrySnapshot>> Handle(GetTelemetryHistoryByDeviceIdQuery query)
    {
        var activeReg = await _registrationRepository.FindByObd2DeviceIdAndStatusAsync(
            query.DeviceId, OBD2DeviceRegistrationStatus.ACTIVE);

        if (activeReg == null)
        {
            return Enumerable.Empty<TelemetrySnapshot>();
        }

        var fromDate = query.From ?? DateTime.UtcNow.AddDays(-30);
        var toDate = query.To ?? DateTime.UtcNow;

        return await _snapshotRepository.FindByRegistrationIdAndDateRangeAsync(
            activeReg.Id, fromDate, toDate);
    }
}
