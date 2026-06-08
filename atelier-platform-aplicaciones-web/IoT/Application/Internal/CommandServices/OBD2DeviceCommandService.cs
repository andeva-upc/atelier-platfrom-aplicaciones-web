using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.Shared.Application.Patterns;
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

    public async Task<Result<OBD2Device, string>> Handle(CreateOBD2DeviceCommand command)
    {
        var existingDevice = await _deviceRepository.FindByMacAddressAsync(command.MacAddress);
        if (existingDevice != null)
        {
            return new Result<OBD2Device, string>.Failure("iot.error.device.macExists");
        }

        var device = new OBD2Device(command.BranchId, command.MacAddress);
        await _deviceRepository.AddAsync(device);
        await _unitOfWork.CompleteAsync();

        return new Result<OBD2Device, string>.Success(device);
    }

    public async Task<Result<OBD2Device, string>> Handle(UpdateOBD2DeviceCommand command)
    {
        var device = await _deviceRepository.FindByIdAsync(command.Id);
        if (device == null)
        {
            return new Result<OBD2Device, string>.Failure("iot.error.device.notFound");
        }

        var existingWithMac = await _deviceRepository.FindByMacAddressAsync(command.MacAddress);
        if (existingWithMac != null && existingWithMac.Id != command.Id)
        {
            return new Result<OBD2Device, string>.Failure("iot.error.device.macExists");
        }

        device.UpdateDetails(command.BranchId, command.MacAddress);
        _deviceRepository.Update(device);
        await _unitOfWork.CompleteAsync();

        return new Result<OBD2Device, string>.Success(device);
    }

    public async Task<Result<bool, string>> Handle(DeleteOBD2DeviceCommand command)
    {
        var device = await _deviceRepository.FindByIdAsync(command.Id);
        if (device == null)
        {
            return new Result<bool, string>.Failure("iot.error.device.notFound");
        }

        device.MarkAsDeleted();
        _deviceRepository.Update(device);
        await _unitOfWork.CompleteAsync();

        return new Result<bool, string>.Success(true);
    }

    public async Task<Result<OBD2DeviceRegistration, string>> Handle(LinkDeviceCommand command)
    {
        var device = await _deviceRepository.FindByIdAsync(command.Obd2DeviceId);
        if (device == null)
        {
            return new Result<OBD2DeviceRegistration, string>.Failure("iot.error.device.notFound");
        }

        if (device.Status != OBD2DeviceStatus.AVAILABLE)
        {
            return new Result<OBD2DeviceRegistration, string>.Failure("iot.error.device.notAvailableForLinking");
        }

        var activeDeviceReg = await _registrationRepository.FindByObd2DeviceIdAndStatusAsync(
            command.Obd2DeviceId, OBD2DeviceRegistrationStatus.ACTIVE);
        if (activeDeviceReg != null)
        {
            return new Result<OBD2DeviceRegistration, string>.Failure("iot.error.device.alreadyLinked");
        }

        var activeVehicleReg = await _registrationRepository.FindByVehicleIdAndStatusAsync(
            command.VehicleId, OBD2DeviceRegistrationStatus.ACTIVE);
        if (activeVehicleReg != null)
        {
            return new Result<OBD2DeviceRegistration, string>.Failure("iot.error.vehicle.alreadyHasDevice");
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

        return new Result<OBD2DeviceRegistration, string>.Success(registration);
    }

    public async Task<Result<OBD2DeviceRegistration, string>> Handle(UnlinkDeviceCommand command)
    {
        var activeReg = await _registrationRepository.FindByObd2DeviceIdAndStatusAsync(
            command.Obd2DeviceId, OBD2DeviceRegistrationStatus.ACTIVE);
        if (activeReg == null)
        {
            return new Result<OBD2DeviceRegistration, string>.Failure("iot.error.registration.notFound");
        }

        var device = await _deviceRepository.FindByIdAsync(command.Obd2DeviceId);
        if (device == null)
        {
            return new Result<OBD2DeviceRegistration, string>.Failure("iot.error.device.notFound");
        }

        activeReg.Deactivate();
        device.Unlink();

        _registrationRepository.Update(activeReg);
        _deviceRepository.Update(device);
        await _unitOfWork.CompleteAsync();

        return new Result<OBD2DeviceRegistration, string>.Success(activeReg);
    }
}
