using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using atelier_platform_aplicaciones_web.Operations.Application.Services;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace atelier_platform_aplicaciones_web.Operations.Application.Internal.QueryServices;

public class WorkOrderQueryService : IWorkOrderQueryService
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly AppDbContext _context;

    public WorkOrderQueryService(IWorkOrderRepository workOrderRepository, AppDbContext context)
    {
        _workOrderRepository = workOrderRepository;
        _context = context;
    }

    public async Task<WorkOrder?> Handle(GetWorkOrderByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _workOrderRepository.FindByIdWithTasksAndProductsAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> Handle(GetWorkOrdersByBranchIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _workOrderRepository.FindByBranchIdAsync(query.BranchId, cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> Handle(GetWorkOrdersByVehicleIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _workOrderRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);
    }
    
    public string GetBranchCode(Guid branchId)
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT code FROM branches WHERE id = @id";
            
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = branchId;
            command.Parameters.Add(parameter);
            bool wasOpen = connection.State == ConnectionState.Open;
            if (!wasOpen) connection.Open();
            var result = command.ExecuteScalar();
            
            if (!wasOpen) connection.Close();
            return result?.ToString() ?? "WO";
        }
        catch
        {
            return "WO";
        }
    }
}