using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class TelemetryCommandService : ITelemetryCommandService
{
    private readonly IOBD2DeviceRegistrationRepository _registrationRepository;
    private readonly ITelemetrySnapshotRepository _snapshotRepository;
    private readonly IOBD2DeviceRepository _deviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TelemetryCommandService> _logger;

    public TelemetryCommandService(
        IOBD2DeviceRegistrationRepository registrationRepository,
        ITelemetrySnapshotRepository snapshotRepository,
        IOBD2DeviceRepository deviceRepository,
        IUnitOfWork unitOfWork,
        ILogger<TelemetryCommandService> logger)
    {
        _registrationRepository = registrationRepository;
        _snapshotRepository = snapshotRepository;
        _deviceRepository = deviceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(IngestTelemetryBatchCommand command)
    {
        var activeReg = await _registrationRepository.FindByObd2DeviceIdAndStatusAsync(
            command.DeviceId, OBD2DeviceRegistrationStatus.ACTIVE);

        if (activeReg == null)
        {
            _logger.LogWarning("Discarding telemetry batch: Device {DeviceId} is not actively linked to any vehicle.", command.DeviceId);
            return;
        }

        int? rpm = null;
        int? temp = null;
        double? speed = null;
        int? odometer = null;
        double? fuel = null;

        foreach (var reading in command.Readings)
        {
            switch (reading.Parameter.ToUpperInvariant())
            {
                case "RPM":
                    rpm = (int)reading.Value;
                    break;
                case "TEMP_ENGINE":
                    temp = (int)reading.Value;
                    break;
                case "SPEED_KMH":
                    speed = reading.Value;
                    break;
                case "ODOMETER_KM":
                    odometer = (int)reading.Value;
                    break;
                case "FUEL_LEVEL":
                    fuel = reading.Value;
                    break;
            }
        }

        var snapshot = new TelemetrySnapshot(
            activeReg.Id,
            activeReg.BranchId,
            rpm,
            temp,
            speed,
            odometer,
            fuel
        );

        await _snapshotRepository.AddAsync(snapshot);

        var device = await _deviceRepository.FindByIdAsync(command.DeviceId);
        if (device != null)
        {
            device.RecordPing();
            _deviceRepository.Update(device);
        }

        await _unitOfWork.CompleteAsync();
    }
}
