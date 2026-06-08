using atelier_platform_aplicaciones_web.IoT.Domain.Model;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class OBD2DeviceCommandService : IOBD2DeviceCommandService
{
    private readonly IOBD2DeviceRepository _deviceRepository;
    private readonly IOBD2DeviceRegistrationRepository _registrationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OBD2DeviceCommandService(
        IOBD2DeviceRepository deviceRepository,
        IOBD2DeviceRegistrationRepository registrationRepository,
        IUnitOfWork unitOfWork)
    {
        _deviceRepository = deviceRepository;
        _registrationRepository = registrationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OBD2Device>> Handle(CreateOBD2DeviceCommand command)
    {
        var existingDevice = await _deviceRepository.FindByMacAddressAsync(command.MacAddress);
        if (existingDevice != null)
        {
            return Result<OBD2Device>.Failure(IoTError.Duplicate, "iot.error.device.macExists");
        }

        var device = new OBD2Device(command.BranchId, command.MacAddress);
        await _deviceRepository.AddAsync(device);
        await _unitOfWork.CompleteAsync();

        return Result<OBD2Device>.Success(device);
    }

    public async Task<Result<OBD2Device>> Handle(UpdateOBD2DeviceCommand command)
    {
        var device = await _deviceRepository.FindByIdAsync(command.Id);
        if (device == null)
        {
            return Result<OBD2Device>.Failure(IoTError.NotFound, "iot.error.device.notFound");
        }

        var existingWithMac = await _deviceRepository.FindByMacAddressAsync(command.MacAddress);
        if (existingWithMac != null && existingWithMac.Id != command.Id)
        {
            return Result<OBD2Device>.Failure(IoTError.Duplicate, "iot.error.device.macExists");
        }

        device.UpdateDetails(command.BranchId, command.MacAddress);
        _deviceRepository.Update(device);
        await _unitOfWork.CompleteAsync();

        return Result<OBD2Device>.Success(device);
    }

    public async Task<Result<bool>> Handle(DeleteOBD2DeviceCommand command)
    {
        var device = await _deviceRepository.FindByIdAsync(command.Id);
        if (device == null)
        {
            return Result<bool>.Failure(IoTError.NotFound, "iot.error.device.notFound");
        }

        device.MarkAsDeleted();
        _deviceRepository.Update(device);
        await _unitOfWork.CompleteAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<OBD2DeviceRegistration>> Handle(LinkDeviceCommand command)
    {
        var device = await _deviceRepository.FindByIdAsync(command.Obd2DeviceId);
        if (device == null)
        {
            return Result<OBD2DeviceRegistration>.Failure(IoTError.NotFound, "iot.error.device.notFound");
        }

        if (device.Status != OBD2DeviceStatus.AVAILABLE)
        {
            return Result<OBD2DeviceRegistration>.Failure(IoTError.InvalidState, "iot.error.device.notAvailableForLinking");
        }

        var activeDeviceReg = await _registrationRepository.FindByObd2DeviceIdAndStatusAsync(
            command.Obd2DeviceId, OBD2DeviceRegistrationStatus.ACTIVE);
        if (activeDeviceReg != null)
        {
            return Result<OBD2DeviceRegistration>.Failure(IoTError.Duplicate, "iot.error.device.alreadyLinked");
        }

        var activeVehicleReg = await _registrationRepository.FindByVehicleIdAndStatusAsync(
            command.VehicleId, OBD2DeviceRegistrationStatus.ACTIVE);
        if (activeVehicleReg != null)
        {
            return Result<OBD2DeviceRegistration>.Failure(IoTError.Duplicate, "iot.error.vehicle.alreadyHasDevice");
        }

        device.Link();
        _deviceRepository.Update(device);

        var registration = new OBD2DeviceRegistration(
            command.Obd2DeviceId,
            command.BranchId,
            command.VehicleId
        );

        await _registrationRepository.AddAsync(registration);
        await _unitOfWork.CompleteAsync();

        return Result<OBD2DeviceRegistration>.Success(registration);
    }

    public async Task<Result<OBD2DeviceRegistration>> Handle(UnlinkDeviceCommand command)
    {
        var activeReg = await _registrationRepository.FindByObd2DeviceIdAndStatusAsync(
            command.Obd2DeviceId, OBD2DeviceRegistrationStatus.ACTIVE);
        if (activeReg == null)
        {
            return Result<OBD2DeviceRegistration>.Failure(IoTError.NotFound, "iot.error.registration.notFound");
        }

        var device = await _deviceRepository.FindByIdAsync(command.Obd2DeviceId);
        if (device == null)
        {
            return Result<OBD2DeviceRegistration>.Failure(IoTError.NotFound, "iot.error.device.notFound");
        }

        activeReg.Deactivate();
        device.Unlink();

        _registrationRepository.Update(activeReg);
        _deviceRepository.Update(device);
        await _unitOfWork.CompleteAsync();

        return Result<OBD2DeviceRegistration>.Success(activeReg);
    }
}
