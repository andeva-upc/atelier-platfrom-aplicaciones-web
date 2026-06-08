using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class VehicleQueryService : IVehicleQueryService
{
    private readonly IVehicleRegistrationRepository _vehicleRegistrationRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleQueryService(
        IVehicleRegistrationRepository vehicleRegistrationRepository,
        IVehicleRepository vehicleRepository)
    {
        _vehicleRegistrationRepository = vehicleRegistrationRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IEnumerable<Vehicle>> Handle(GetVehiclesByBranchIdQuery query)
    {
        var activeRegs = await _vehicleRegistrationRepository.FindActiveByBranchIdAsync(query.BranchId);
        var vehicles = new List<Vehicle>();

        foreach (var reg in activeRegs)
        {
            var vehicle = await _vehicleRepository.FindByIdAsync(reg.VehicleId);
            if (vehicle != null)
            {
                vehicles.Add(vehicle);
            }
        }

        return vehicles;
    }
}
