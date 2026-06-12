using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Repositories;

public interface IWorkOrderRepository : IBaseRepository<WorkOrder>
{
    Task<WorkOrder?> FindByIdWithTasksAndProductsAsync(WorkOrderId id, CancellationToken cancellationToken = default);
    
    Task<WorkOrder?> FindWorkOrderByIdAsync(WorkOrderId id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<WorkOrder>> FindByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<WorkOrder>> FindByVehicleIdAsync(VehicleId vehicleId, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsByAppointmentIdAsync(AppointmentId appointmentId, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsByIdAsync(WorkOrderId id, CancellationToken cancellationToken = default);
    
    Task<int> FindMaxInternalNumberByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default);
}