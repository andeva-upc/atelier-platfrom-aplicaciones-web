using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Domain.Model;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using Microsoft.Extensions.Localization;
using atelier_platform_aplicaciones_web.Operations.Resources;

namespace atelier_platform_aplicaciones_web.Operations.Application.Internal.CommandServices;

public class ServiceCommandService(
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<OperationsMessages> localizer) : IServiceCommandService
{
    public async Task<Result<Service>> Handle(CreateServiceCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var service = new Service(
                command.BranchId,
                command.Name,
                command.Price
            );

            await serviceRepository.AddAsync(service, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Service>.Success(service);
        }
        catch (ArgumentException e)
        {
            return Result<Service>.Failure(WorkOrderError.InvalidState, localizer[e.Message]);
        }
        catch (Exception)
        {
            return Result<Service>.Failure(WorkOrderError.UnexpectedError, localizer["operations.error.unexpected"]);
        }
    }

    public async Task<Result<Service>> Handle(UpdateServiceCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var service = await serviceRepository.FindServiceByIdAsync(command.ServiceId, cancellationToken);
            if (service == null)
            {
                return Result<Service>.Failure(WorkOrderError.NotFound, localizer["operations.error.service.notFound"]);
            }

            service.Update(command.Name, command.Price);

            serviceRepository.Update(service);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Service>.Success(service);
        }
        catch (ArgumentException e)
        {
            return Result<Service>.Failure(WorkOrderError.InvalidState, localizer[e.Message]);
        }
        catch (Exception)
        {
            return Result<Service>.Failure(WorkOrderError.UnexpectedError, localizer["operations.error.unexpected"]);
        }
    }

    public async Task<Result> Handle(DeleteServiceCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var service = await serviceRepository.FindServiceByIdAsync(command.ServiceId, cancellationToken);
            if (service == null)
            {
                return Result.Failure(WorkOrderError.NotFound, localizer["operations.error.service.notFound"]);
            }

            serviceRepository.Remove(service);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(WorkOrderError.UnexpectedError, localizer["operations.error.unexpected"]);
        }
    }
}
