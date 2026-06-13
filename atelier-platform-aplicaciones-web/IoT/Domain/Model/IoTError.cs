namespace atelier_platform_aplicaciones_web.IoT.Domain.Model;

public enum IoTError
{
    None,
    Obd2DeviceNotFound,
    Obd2DeviceAlreadyLinked,
    Obd2DeviceRegistrationNotFound,
    VehicleNotFound,
    VehicleRegistrationNotFound,
    VinAlreadyRegistered,
    PlateNumberAlreadyRegistered,
    DuplicateMacAddress,
    VehicleAlreadyLinked
}
