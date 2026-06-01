using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace atelier_platform_aplicaciones_web.Operations.Infrastructure.Persistence.EFC.Repositories;

public class WorkOrderRepository : BaseRepository<WorkOrder>, IWorkOrderRepository
{
    public WorkOrderRepository(AppDbContext context) : base(context)
    {
    }

    // Eager Loading: Carga la Orden de Trabajo con sus Tareas y los Productos de cada Tarea
    public async Task<WorkOrder?> FindByIdWithTasksAndProductsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<WorkOrder>()
            .Include(w => w.Tasks)
                .ThenInclude(t => t.Products)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByAppointmentIdAsync(AppointmentId appointmentId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<WorkOrder>()
            .AnyAsync(w => w.AppointmentId == appointmentId, cancellationToken);
    }

    public async Task<int> FindMaxInternalNumberByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default)
    {
        var numbers = await Context.Set<WorkOrder>()
            .Where(w => w.BranchId == branchId)
            .Select(w => w.InternalNumber)
            .ToListAsync(cancellationToken);

        return numbers.Any() ? numbers.Max() : 0;
    }

    public async Task<IEnumerable<WorkOrder>> FindByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<WorkOrder>()
            .Include(w => w.Tasks)
                .ThenInclude(t => t.Products)
            .Where(w => w.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> FindByVehicleIdAsync(VehicleId vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<WorkOrder>()
            .Include(w => w.Tasks)
                .ThenInclude(t => t.Products)
            .Where(w => w.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }
}