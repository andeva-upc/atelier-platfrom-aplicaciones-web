using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class Obd2DeviceRegistrationCommandService(
    IObd2DeviceRepository obd2DeviceRepository,
    IObd2DeviceRegistrationRepository obd2DeviceRegistrationRepository,
    IUnitOfWork unitOfWork) : IObd2DeviceRegistrationCommandService
{
    public async Task<Result<Obd2DeviceRegistration>> Handle(LinkObd2DeviceToVehicleCommand command, CancellationToken cancellationToken = default)
    {
        // 1. Validar existencia del OBD2
        var obd2Device = await obd2DeviceRepository.FindObd2DeviceByIdAsync(command.Obd2DeviceId, cancellationToken);
        if (obd2Device == null)
        {
            return Result<Obd2DeviceRegistration>.Failure(IoTError.Obd2DeviceNotFound, "iot.error.obd2Device.notFound");
        }

        // 2. Validar que el OBD2 no esté ya marcado como LINKED en su agregado
        if (obd2Device.Status == Obd2DeviceStatus.Linked)
        {
            return Result<Obd2DeviceRegistration>.Failure(IoTError.Obd2DeviceAlreadyLinked, "iot.error.obd2DeviceRegistration.deviceAlreadyLinked");
        }

        // 3. Validar que el OBD2 no tenga ya una vinculación activa en base de datos
        var activeDeviceReg = await obd2DeviceRegistrationRepository.FindActiveByObd2DeviceIdAsync(command.Obd2DeviceId, cancellationToken);
        if (activeDeviceReg != null)
        {
            return Result<Obd2DeviceRegistration>.Failure(IoTError.Obd2DeviceAlreadyLinked, "iot.error.obd2DeviceRegistration.deviceAlreadyLinked");
        }

        // 4. Validar que el vehículo no tenga una vinculación activa en base de datos
        var activeVehicleReg = await obd2DeviceRegistrationRepository.FindActiveByVehicleIdAsync(command.VehicleId, cancellationToken);
        if (activeVehicleReg != null)
        {
            return Result<Obd2DeviceRegistration>.Failure(IoTError.VehicleAlreadyLinked, "iot.error.obd2DeviceRegistration.vehicleAlreadyLinked");
        }

        // 5. Actualizar el estado del OBD2 a LINKED
        obd2Device.MarkAsLinked();
        obd2DeviceRepository.Update(obd2Device);

        // 6. Crear la vinculación
        var registration = new Obd2DeviceRegistration(command.Obd2DeviceId, command.BranchId, command.VehicleId);
        await obd2DeviceRegistrationRepository.AddAsync(registration, cancellationToken);

        // 7. Completar transacción de forma atómica
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Obd2DeviceRegistration>.Success(registration);
    }
}
