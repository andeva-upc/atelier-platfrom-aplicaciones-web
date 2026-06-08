using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Patterns;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Services;

public interface IOBD2DeviceCommandService
{
    Task<Result<OBD2Device, string>> Handle(CreateOBD2DeviceCommand command);
    Task<Result<OBD2Device, string>> Handle(UpdateOBD2DeviceCommand command);
    Task<Result<bool, string>> Handle(DeleteOBD2DeviceCommand command);
    Task<Result<OBD2DeviceRegistration, string>> Handle(LinkDeviceCommand command);
    Task<Result<OBD2DeviceRegistration, string>> Handle(UnlinkDeviceCommand command);
}
