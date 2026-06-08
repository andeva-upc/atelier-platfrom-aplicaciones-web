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

public class BranchCommandService(
    IBranchRepository branchRepository,
    IWorkshopRepository workshopRepository,
    IUnitOfWork unitOfWork) : IBranchCommandService
{
    public async Task<Result<Branch>> Handle(CreateBranchCommand command, CancellationToken cancellationToken = default)
    {
        if (!await workshopRepository.ExistsByIdAsync(command.WorkshopId, cancellationToken))
        {
            return Result<Branch>.Failure(CoreError.WorkshopNotFound, "core.error.workshop.notFound");
        }

        if (await branchRepository.ExistsByCodeAsync(command.Code, cancellationToken))
        {
            return Result<Branch>.Failure(CoreError.BranchCodeMustBeUnique, "core.error.branch.codeMustBeUnique");
        }

        var branch = new Branch(
            command.WorkshopId,
            command.Code,
            command.Name,
            command.Address,
            command.Phone
        );

        await branchRepository.AddAsync(branch, cancellationToken);
        await unitOfWork.CompleteAsync();

        return Result<Branch>.Success(branch);
    }

    public async Task<Result<Branch>> Handle(UpdateBranchCommand command, CancellationToken cancellationToken = default)
    {
        var branch = await branchRepository.FindBranchByIdAsync(command.Id, cancellationToken);
        if (branch == null)
        {
            return Result<Branch>.Failure(CoreError.BranchNotFound, "core.error.branch.notFound");
        }

        if (branch.Code != command.Code && await branchRepository.ExistsByCodeAsync(command.Code, cancellationToken))
        {
            return Result<Branch>.Failure(CoreError.BranchCodeMustBeUnique, "core.error.branch.codeMustBeUnique");
        }

        branch.Update(
            command.Code,
            command.Name,
            command.Address,
            command.Phone
        );

        branchRepository.Update(branch);
        await unitOfWork.CompleteAsync();

        return Result<Branch>.Success(branch);
    }
}
