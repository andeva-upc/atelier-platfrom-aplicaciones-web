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
using atelier_platform_aplicaciones_web.Billing.Resources;
using Microsoft.Extensions.Localization;

namespace atelier_platform_aplicaciones_web.Billing.Application.Internal.CommandServices;

public enum BillingErrorCodes { CreationFailed, UpdateFailed, QuoteNotFound, ApprovalFailed, CancellationFailed, VoucherGenerationFailed, QuoteNotApproved, FacthubServiceUnavailable, VoucherNotFound, PaymentConflict, BadRequest, InternalError, PaymentNotFound }

public class QuoteCommandService : IQuoteCommandService
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<BillingMessages> _localizer;

    public QuoteCommandService(IQuoteRepository quoteRepository, IUnitOfWork unitOfWork, IStringLocalizer<BillingMessages> localizer)
    {
        _quoteRepository = quoteRepository;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
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
            return Result<Quote>.Failure(BillingErrorCodes.CreationFailed, _localizer["billing.error.unexpected"]);
        }
    }

    public async Task<Result<Quote>> Handle(UpdateQuoteCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var quote = await _quoteRepository.FindByIdAsync(command.Id);
            if (quote == null)
            {
                return Result<Quote>.Failure(BillingErrorCodes.QuoteNotFound, _localizer["billing.error.quote.notFound"]);
            }

            quote.Update(command.SubtotalAmount, command.DiscountPercentage);
            _quoteRepository.Update(quote);
            await _unitOfWork.CompleteAsync();

            return Result<Quote>.Success(quote);
        }
        catch (Exception ex)
        {
            return Result<Quote>.Failure(BillingErrorCodes.UpdateFailed, _localizer["billing.error.unexpected"]);
        }
    }

    public async Task<Result<Quote>> Handle(ApproveQuoteCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var quote = await _quoteRepository.FindByIdAsync(command.Id);
            if (quote == null)
            {
                return Result<Quote>.Failure(BillingErrorCodes.QuoteNotFound, _localizer["billing.error.quote.notFound"]);
            }

            quote.Approve();
            _quoteRepository.Update(quote);
            await _unitOfWork.CompleteAsync();

            return Result<Quote>.Success(quote);
        }
        catch (Exception ex)
        {
            return Result<Quote>.Failure(BillingErrorCodes.ApprovalFailed, _localizer["billing.error.unexpected"]);
        }
    }

    public async Task<Result<Quote>> Handle(CancelQuoteCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var quote = await _quoteRepository.FindByIdAsync(command.Id);
            if (quote == null)
            {
                return Result<Quote>.Failure(BillingErrorCodes.QuoteNotFound, _localizer["billing.error.quote.notFound"]);
            }

            quote.Cancel();
            _quoteRepository.Update(quote);
            await _unitOfWork.CompleteAsync();

            return Result<Quote>.Success(quote);
        }
        catch (Exception ex)
        {
            return Result<Quote>.Failure(BillingErrorCodes.CancellationFailed, _localizer["billing.error.unexpected"]);
        }
    }
}
