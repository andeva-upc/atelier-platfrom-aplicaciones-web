using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.Shared.Application.Patterns;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class VehicleCommandService : IVehicleCommandService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IVehicleRegistrationRepository _vehicleRegistrationRepository;
    private readonly IOBD2DeviceRegistrationRepository _obd2RegistrationRepository;
    private readonly IOBD2DeviceRepository _obd2DeviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VehicleCommandService(
        IVehicleRepository vehicleRepository,
        IVehicleRegistrationRepository vehicleRegistrationRepository,
        IOBD2DeviceRegistrationRepository obd2RegistrationRepository,
        IOBD2DeviceRepository obd2DeviceRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _vehicleRegistrationRepository = vehicleRegistrationRepository;
        _obd2RegistrationRepository = obd2RegistrationRepository;
        _obd2DeviceRepository = obd2DeviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Vehicle, string>> Handle(RegisterVehicleCommand command)
    {
        var vehicle = await _vehicleRepository.FindByVinAsync(command.Vin);
        bool isNewVehicle = false;

        if (vehicle == null)
        {
            vehicle = new Vehicle(
                command.PlateNumber,
                command.Vin,
                command.Year,
                command.Brand,
                command.Model
            );
            await _vehicleRepository.AddAsync(vehicle);
            isNewVehicle = true;
        }
        else
        {
            // If vehicle exists, update its details
            vehicle.UpdateDetails(command.PlateNumber, command.Year, command.Brand, command.Model);
            _vehicleRepository.Update(vehicle);

            // Deactivate previous active owner registrations for this vehicle
            var activeOwnerReg = await _vehicleRegistrationRepository.FindByVehicleIdAndStatusAsync(
                vehicle.Id, VehicleRegistrationStatus.ACTIVE);

            if (activeOwnerReg != null)
            {
                activeOwnerReg.MarkAsPrevious();
                _vehicleRegistrationRepository.Update(activeOwnerReg);

                // Cascade OBD2 deactivation logic:
                // Find any active OBD2 device registration for this vehicle_id
                var activeObd2Reg = await _obd2RegistrationRepository.FindByVehicleIdAndStatusAsync(
                    vehicle.Id, OBD2DeviceRegistrationStatus.ACTIVE);

                if (activeObd2Reg != null)
                {
                    activeObd2Reg.Deactivate();
                    _obd2RegistrationRepository.Update(activeObd2Reg);

                    // Revert the physical OBD2 device back to AVAILABLE
                    var obd2Device = await _obd2DeviceRepository.FindByIdAsync(activeObd2Reg.Obd2DeviceId);
                    if (obd2Device != null)
                    {
                        obd2Device.Unlink();
                        _obd2DeviceRepository.Update(obd2Device);
                    }
                }
            }
        }

        // Create new active owner registration
        var registration = new VehicleRegistration(command.UserId, vehicle.Id);
        await _vehicleRegistrationRepository.AddAsync(registration);

        await _unitOfWork.CompleteAsync();

        return new Result<Vehicle, string>.Success(vehicle);
    }
}
