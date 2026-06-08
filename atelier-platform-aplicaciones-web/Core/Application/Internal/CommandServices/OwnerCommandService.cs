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

public class OwnerCommandService(
    IOwnerRepository ownerRepository,
    IUnitOfWork unitOfWork) : IOwnerCommandService
{
    public async Task<Result<Owner>> Handle(CreateOwnerCommand command, CancellationToken cancellationToken = default)
    {
        if (await ownerRepository.ExistsByUserIdAsync(command.UserId, cancellationToken))
        {
            return Result<Owner>.Failure(CoreError.OwnerProfileAlreadyExists, "core.error.owner.profileAlreadyExists");
        }

        var owner = new Owner(
            command.UserId,
            command.Name,
            command.Document,
            command.Phone
        );

        await ownerRepository.AddAsync(owner, cancellationToken);
        await unitOfWork.CompleteAsync();

        return Result<Owner>.Success(owner);
    }

    public async Task<Result<Owner>> Handle(UpdateOwnerCommand command, CancellationToken cancellationToken = default)
    {
        var owner = await ownerRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (owner == null)
        {
            return Result<Owner>.Failure(CoreError.OwnerNotFound, "core.error.owner.notFound");
        }

        owner.Update(
            command.Name,
            command.Document,
            command.Phone
        );

        ownerRepository.Update(owner);
        await unitOfWork.CompleteAsync();

        return Result<Owner>.Success(owner);
    }

    public async Task<Result> Handle(DeleteOwnerCommand command, CancellationToken cancellationToken = default)
    {
        var owner = await ownerRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (owner == null)
        {
            return Result.Failure(CoreError.OwnerNotFound, "core.error.owner.notFound");
        }

        ownerRepository.Remove(owner);
        await unitOfWork.CompleteAsync();

        return Result.Success();
    }
}
