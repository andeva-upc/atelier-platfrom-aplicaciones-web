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

public class CustomerCommandService(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork) : ICustomerCommandService
{
    public async Task<Result<Customer>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        if (await customerRepository.ExistsByUserIdAsync(command.UserId, cancellationToken))
        {
            return Result<Customer>.Failure(CoreError.CustomerProfileAlreadyExists, "core.error.customer.profileAlreadyExists");
        }

        var customer = new Customer(
            command.UserId,
            command.IsCorporate,
            command.Name,
            command.BusinessName,
            command.Document,
            command.Phone
        );

        await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.CompleteAsync();

        return Result<Customer>.Success(customer);
    }

    public async Task<Result<Customer>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (customer == null)
        {
            return Result<Customer>.Failure(CoreError.CustomerNotFound, "core.error.customer.notFound");
        }

        customer.Update(
            command.Name,
            command.BusinessName,
            command.Document,
            command.Phone
        );

        customerRepository.Update(customer);
        await unitOfWork.CompleteAsync();

        return Result<Customer>.Success(customer);
    }

    public async Task<Result> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (customer == null)
        {
            return Result.Failure(CoreError.CustomerNotFound, "core.error.customer.notFound");
        }

        customerRepository.Remove(customer);
        await unitOfWork.CompleteAsync();

        return Result.Success();
    }
}
