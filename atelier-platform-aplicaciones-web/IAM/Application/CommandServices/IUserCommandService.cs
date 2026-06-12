using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IAM.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.IAM.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<AuthenticatedUser>> Handle(SignInCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(SignUpCommand command, CancellationToken cancellationToken);
    Task<Result<AuthenticatedUser>> Handle(UpdateUserEmailCommand command, CancellationToken cancellationToken);
    Task<Result<User>> Handle(UpdateUserPasswordCommand command, CancellationToken cancellationToken);
    Task<Result<AuthenticatedUser>> Handle(GoogleSignInCommand command, CancellationToken cancellationToken);
}
