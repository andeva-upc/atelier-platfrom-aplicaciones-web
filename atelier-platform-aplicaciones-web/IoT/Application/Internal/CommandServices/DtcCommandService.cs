using atelier_platform_aplicaciones_web.IoT.Domain.Model;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class DtcCommandService : IDtcCommandService
{
    private readonly IDtcAlertRepository _dtcAlertRepository;
    private readonly IOBD2DeviceRegistrationRepository _registrationRepository;
    private readonly ITelemetrySnapshotRepository _snapshotRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DtcCommandService(
        IDtcAlertRepository dtcAlertRepository,
        IOBD2DeviceRegistrationRepository registrationRepository,
        ITelemetrySnapshotRepository snapshotRepository,
        IUnitOfWork unitOfWork)
    {
        _dtcAlertRepository = dtcAlertRepository;
        _registrationRepository = registrationRepository;
        _snapshotRepository = snapshotRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DtcAlert>> Handle(ReportDtcErrorCommand command)
    {
        var activeReg = await _registrationRepository.FindByObd2DeviceIdAndStatusAsync(
            command.DeviceId, OBD2DeviceRegistrationStatus.ACTIVE);

        if (activeReg == null)
        {
            return Result<DtcAlert>.Failure(IoTError.NotFound, "iot.error.registration.notFoundForDevice");
        }

        var latestSnapshot = await _snapshotRepository.FindLatestByRegistrationIdAsync(activeReg.Id);
        if (latestSnapshot == null)
        {
            return Result<DtcAlert>.Failure(IoTError.InvalidState, "iot.error.dtc.noTelemetryAvailable");
        }

        DtcCode dtcCode;
        try
        {
            dtcCode = new DtcCode(command.DtcCode);
        }
        catch (ArgumentException ex)
        {
            return Result<DtcAlert>.Failure(IoTError.InvalidState, ex.Message);
        }

        var alert = new DtcAlert(
            latestSnapshot.Id,
            activeReg.BranchId,
            dtcCode,
            command.Description
        );

        await _dtcAlertRepository.AddAsync(alert);
        await _unitOfWork.CompleteAsync();

        return Result<DtcAlert>.Success(alert);
    }
}
