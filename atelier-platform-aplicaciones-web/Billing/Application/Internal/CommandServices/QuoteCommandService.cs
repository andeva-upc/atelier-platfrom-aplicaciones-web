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

public enum BillingErrorCodes { CreationFailed, UpdateFailed, QuoteNotFound, ApprovalFailed, CancellationFailed }

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
            var quote = new Quote(
            command.WorkOrderId,
            command.BranchId,
            command.SubtotalAmount,
            command.DiscountPercentage
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

    public async Task<Result<Quote>> Handle(UpdateQuoteCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var quote = await _quoteRepository.FindByIdAsync(command.Id);
            if (quote == null)
            {
                return Result<Quote>.Failure(BillingErrorCodes.QuoteNotFound, $"Quote with ID {command.Id} not found.");
            }

            quote.Update(command.SubtotalAmount, command.DiscountPercentage);
            _quoteRepository.Update(quote);
            await _unitOfWork.CompleteAsync();

            return Result<Quote>.Success(quote);
        }
        catch (Exception ex)
        {
            return Result<Quote>.Failure(BillingErrorCodes.UpdateFailed, $"An error occurred while updating the quote: {ex.Message}");
        }
    }

    public async Task<Result<Quote>> Handle(ApproveQuoteCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var quote = await _quoteRepository.FindByIdAsync(command.Id);
            if (quote == null)
            {
                return Result<Quote>.Failure(BillingErrorCodes.QuoteNotFound, $"Quote with ID {command.Id} not found.");
            }

            quote.Approve();
            _quoteRepository.Update(quote);
            await _unitOfWork.CompleteAsync();

            return Result<Quote>.Success(quote);
        }
        catch (Exception ex)
        {
            return Result<Quote>.Failure(BillingErrorCodes.ApprovalFailed, $"An error occurred while approving the quote: {ex.Message}");
        }
    }

    public async Task<Result<Quote>> Handle(CancelQuoteCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var quote = await _quoteRepository.FindByIdAsync(command.Id);
            if (quote == null)
            {
                return Result<Quote>.Failure(BillingErrorCodes.QuoteNotFound, $"Quote with ID {command.Id} not found.");
            }

            quote.Cancel();
            _quoteRepository.Update(quote);
            await _unitOfWork.CompleteAsync();

            return Result<Quote>.Success(quote);
        }
        catch (Exception ex)
        {
            return Result<Quote>.Failure(BillingErrorCodes.CancellationFailed, $"An error occurred while cancelling the quote: {ex.Message}");
        }
    }
}
