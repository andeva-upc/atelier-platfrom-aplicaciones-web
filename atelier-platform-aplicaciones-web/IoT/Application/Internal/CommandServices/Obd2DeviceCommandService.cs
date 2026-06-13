using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
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
}
