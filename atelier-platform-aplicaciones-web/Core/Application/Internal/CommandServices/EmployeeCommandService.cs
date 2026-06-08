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

public class EmployeeCommandService(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IEmployeeCommandService
{
    public async Task<Result<Employee>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken = default)
    {
        if (await employeeRepository.ExistsByUserIdAsync(command.UserId, cancellationToken))
        {
            return Result<Employee>.Failure(CoreError.EmployeeProfileAlreadyExists, "core.error.employee.profileAlreadyExists");
        }

        var employee = new Employee(
            command.UserId,
            command.Name,
            command.Document,
            command.Phone
        );

        await employeeRepository.AddAsync(employee, cancellationToken);
        await unitOfWork.CompleteAsync();

        return Result<Employee>.Success(employee);
    }

    public async Task<Result<Employee>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (employee == null)
        {
            return Result<Employee>.Failure(CoreError.EmployeeNotFound, "core.error.employee.notFound");
        }

        employee.Update(
            command.Name,
            command.Document,
            command.Phone
        );

        employeeRepository.Update(employee);
        await unitOfWork.CompleteAsync();

        return Result<Employee>.Success(employee);
    }

    public async Task<Result> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (employee == null)
        {
            return Result.Failure(CoreError.EmployeeNotFound, "core.error.employee.notFound");
        }

        employeeRepository.Remove(employee);
        await unitOfWork.CompleteAsync();

        return Result.Success();
    }
}
