using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Services;

public interface IOBD2DeviceCommandService
{
    Task<Result<OBD2Device>> Handle(CreateOBD2DeviceCommand command);
    Task<Result<OBD2Device>> Handle(UpdateOBD2DeviceCommand command);
    Task<Result<bool>> Handle(DeleteOBD2DeviceCommand command);
    Task<Result<OBD2DeviceRegistration>> Handle(LinkDeviceCommand command);
    Task<Result<OBD2DeviceRegistration>> Handle(UnlinkDeviceCommand command);
}
