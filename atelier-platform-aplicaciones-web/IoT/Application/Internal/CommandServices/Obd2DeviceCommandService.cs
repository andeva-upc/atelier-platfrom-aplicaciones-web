using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;

public class Obd2DeviceCommandService(
    IObd2DeviceRepository obd2DeviceRepository,
    IUnitOfWork unitOfWork) : IObd2DeviceCommandService
{
    public async Task<Result<Obd2Device>> Handle(CreateObd2DeviceCommand command, CancellationToken cancellationToken = default)
    {
        if (await obd2DeviceRepository.ExistsByMacAddressAsync(command.MacAddress, cancellationToken))
        {
            return Result<Obd2Device>.Failure(IoTError.DuplicateMacAddress, "iot.error.macAddress.duplicate");
        }

        var obd2Device = new Obd2Device(command.BranchId, command.MacAddress);

        await obd2DeviceRepository.AddAsync(obd2Device, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Obd2Device>.Success(obd2Device);
    }

    public async Task<Result<Obd2Device>> Handle(UpdateObd2DeviceCommand command, CancellationToken cancellationToken = default)
    {
        var obd2Device = await obd2DeviceRepository.FindObd2DeviceByIdAsync(command.Id, cancellationToken);
        if (obd2Device == null)
        {
            return Result<Obd2Device>.Failure(IoTError.Obd2DeviceNotFound, "iot.error.obd2Device.notFound");
        }

        if (obd2Device.Status != Obd2DeviceStatus.Available)
        {
            return Result<Obd2Device>.Failure(IoTError.Obd2DeviceAlreadyLinked, "iot.error.obd2Device.cannotUpdateLinkedDevice");
        }

        if (obd2Device.MacAddress.ToLower() != command.MacAddress.ToLower() &&
            await obd2DeviceRepository.ExistsByMacAddressAsync(command.MacAddress, cancellationToken))
        {
            return Result<Obd2Device>.Failure(IoTError.DuplicateMacAddress, "iot.error.macAddress.duplicate");
        }

        obd2Device.UpdateMacAddress(command.MacAddress);

        obd2DeviceRepository.Update(obd2Device);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<Obd2Device>.Success(obd2Device);
    }
}
