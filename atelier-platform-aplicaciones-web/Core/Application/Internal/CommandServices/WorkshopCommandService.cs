using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.CommandServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Application.Internal.CommandServices;

public class WorkshopCommandService(
    IWorkshopRepository workshopRepository,
    IOwnerRepository ownerRepository,
    IUnitOfWork unitOfWork) : IWorkshopCommandService
{
    public async Task<Result<Workshop>> Handle(CreateWorkshopCommand command, CancellationToken cancellationToken = default)
    {
        if (!await ownerRepository.ExistsByIdAsync(command.OwnerId, cancellationToken))
        {
            return Result<Workshop>.Failure(CoreError.OwnerNotFound, "core.error.owner.notFound");
        }

        var workshop = new Workshop(
            command.OwnerId,
            command.BusinessName,
            command.BrandName,
            command.TaxId,
            command.MileageIntervalConfig
        );

        await workshopRepository.AddAsync(workshop, cancellationToken);
        await unitOfWork.CompleteAsync();

        return Result<Workshop>.Success(workshop);
    }

    public async Task<Result<Workshop>> Handle(UpdateWorkshopCommand command, CancellationToken cancellationToken = default)
    {
        var workshop = await workshopRepository.FindWorkshopByIdAsync(command.Id, cancellationToken);
        if (workshop == null)
        {
            return Result<Workshop>.Failure(CoreError.WorkshopNotFound, "core.error.workshop.notFound");
        }

        workshop.Update(
            command.BusinessName,
            command.BrandName,
            command.TaxId,
            command.MileageIntervalConfig
        );

        workshopRepository.Update(workshop);
        await unitOfWork.CompleteAsync();

        return Result<Workshop>.Success(workshop);
    }
}
