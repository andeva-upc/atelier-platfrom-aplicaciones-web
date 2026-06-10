using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Billing.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Billing.Application.Internal.CommandServices;

public enum BillingErrorCodes { CreationFailed }

public class QuoteCommandService : IQuoteCommandService
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public QuoteCommandService(IQuoteRepository quoteRepository, IUnitOfWork unitOfWork)
    {
        _quoteRepository = quoteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Quote>> Handle(CreateQuoteCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var subtotal = new Money(command.SubtotalAmount, command.Currency);
            var taxRate = new TaxRate(command.TaxPercentage);

            var quote = new Quote(
                command.WorkshopId,
                command.CustomerId,
                command.VehicleId,
                command.Description,
                command.Currency,
                subtotal,
                taxRate
            );

            await _quoteRepository.AddAsync(quote);
            await _unitOfWork.CompleteAsync();

            return Result<Quote>.Success(quote);
        }
        catch (Exception ex)
        {
            return Result<Quote>.Failure(BillingErrorCodes.CreationFailed, $"An error occurred while creating the quote: {ex.Message}");
        }
    }
}
