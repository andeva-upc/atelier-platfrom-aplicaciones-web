using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Application.OutboundServices;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Domain.Repositories;
using atelier_platform_aplicaciones_web.Billing.Infrastructure.ExternalServices.Facthub.Models;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Billing.Resources;
using Microsoft.Extensions.Localization;

namespace atelier_platform_aplicaciones_web.Billing.Application.Internal.CommandServices;

public class VoucherCommandService : IVoucherCommandService
{
    private readonly IVoucherRepository _voucherRepository;
    private readonly IQuoteRepository _quoteRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IWorkshopRepository _workshopRepository;
    private readonly IFacthubService _facthubService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<BillingMessages> _localizer;

    public VoucherCommandService(
        IVoucherRepository voucherRepository, 
        IQuoteRepository quoteRepository,
        IWorkOrderRepository workOrderRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IWorkshopRepository workshopRepository,
        IFacthubService facthubService,
        IUnitOfWork unitOfWork,
        IStringLocalizer<BillingMessages> localizer)
    {
        _voucherRepository = voucherRepository;
        _quoteRepository = quoteRepository;
        _workOrderRepository = workOrderRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _workshopRepository = workshopRepository;
        _facthubService = facthubService;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<Voucher>> Handle(GenerateVoucherCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Gather all required data for Facthub
            var quote = await _quoteRepository.FindByIdAsync(command.QuoteId);
            if (quote == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.quote.notFound"]);

            if (quote.Status != QuoteStatus.APPROVED)
            {
                return Result<Voucher>.Failure(BillingErrorCodes.QuoteNotApproved, _localizer["billing.error.quote.notApproved"]);
            }

            var workOrder = await _workOrderRepository.FindByIdWithTasksAndProductsAsync(quote.WorkOrderId);
            if (workOrder == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.workOrder.notFound"]);

            var branch = await _branchRepository.FindByIdAsync(quote.BranchId);
            if (branch == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.branch.notFound"]);

            var workshop = await _workshopRepository.FindByIdAsync(branch.WorkshopId.Value);
            if (workshop == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.workshop.notFound"]);

            var items = workOrder.Tasks.Select(t => new FacthubItem 
            { 
                Description = t.Description.Value, 
                Quantity = 1, 
                UnitPrice = t.Price.Amount 
            }).ToList();

            // 2. Prepare Facthub Request
            var request = new FacthubIssueRequest
            {
                IssuerRuc = workshop.TaxId,
                DocumentType = command.Type,
                CustomerDocumentType = command.CustomerDocumentType,
                CustomerDocumentNumber = command.CustomerDocumentNumber,
                CustomerName = command.CustomerName,
                Items = items
            };

            // 3. Call Facthub Service
            var response = await _facthubService.IssueInvoiceAsync(request);

            if (response == null || !response.Success || response.Invoice == null)
            {
                return Result<Voucher>.Failure(BillingErrorCodes.FacthubServiceUnavailable, _localizer["billing.error.facthub.unavailable"]);
            }

            // 4. Create Voucher with Facthub data
            Guid? externalInvoiceId = Guid.TryParse(response.Invoice.Id, out var parsedId) ? parsedId : null;
            var voucher = new Voucher(
                command.QuoteId,
                quote.BranchId,
                response.Invoice.Number, // We use the number from Facthub as voucher number if needed, or we could generate our own
                quote.SubtotalAmount,
                command.Type,
                "PEN",
                command.CustomerDocumentType,
                command.CustomerDocumentNumber,
                command.CustomerName,
                externalInvoiceId
            );

            await _voucherRepository.AddAsync(voucher);
            await _unitOfWork.CompleteAsync();

            return Result<Voucher>.Success(voucher);
        }
        catch (Exception ex)
        {
            var innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
            return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.internal"]);
        }
    }
    public async Task<Result<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>> Handle(AddPaymentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var voucher = await _voucherRepository.FindByIdWithPaymentsAsync(command.VoucherId);
            if (voucher == null) return Result<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>.Failure(BillingErrorCodes.VoucherNotFound, _localizer["billing.error.voucher.notFound"]);

            voucher.AddPayment(command.Amount, command.Method, voucher.Currency);
            
            _voucherRepository.Update(voucher);
            await _unitOfWork.CompleteAsync();

            var paymentAdded = voucher.Payments.OrderByDescending(p => p.PaidAt).FirstOrDefault();
            return Result<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>.Success(paymentAdded!);
        }
        catch (Exception ex)
        {
            if (ex.Message == "Voucher is already fully paid." || ex.Message == "Cannot add payment to a canceled voucher.")
                return Result<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>.Failure(BillingErrorCodes.PaymentConflict, _localizer["billing.error.payment.conflict"]);
            
            if (ex.Message == "Payment amount exceeds the remaining balance.")
                return Result<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>.Failure(BillingErrorCodes.BadRequest, _localizer["billing.error.payment.conflict"]);

            return Result<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>.Failure(BillingErrorCodes.InternalError, _localizer["billing.error.internal"]);
        }
    }

    public async Task<Result> Handle(RemovePaymentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var voucher = await _voucherRepository.FindByIdWithPaymentsAsync(command.VoucherId);
            if (voucher == null) return Result.Failure(BillingErrorCodes.VoucherNotFound, _localizer["billing.error.voucher.notFound"]);

            voucher.RemovePayment(command.PaymentId);
            
            _voucherRepository.Update(voucher);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            if (ex.Message == "Payment not found.")
                return Result.Failure(BillingErrorCodes.PaymentNotFound, _localizer["billing.error.payment.notFound"]);

            return Result.Failure(BillingErrorCodes.InternalError, _localizer["billing.error.internal"]);
        }
    }

    public async Task<Result<Voucher>> Handle(ProcessCheckoutCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Validate Quote
            var quote = await _quoteRepository.FindByIdAsync(command.QuoteId);
            if (quote == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.quote.notFound"]);

            if (quote.Status != QuoteStatus.APPROVED)
            {
                return Result<Voucher>.Failure(BillingErrorCodes.QuoteNotApproved, _localizer["billing.error.quote.notApproved"]);
            }

            var workOrder = await _workOrderRepository.FindByIdWithTasksAndProductsAsync(quote.WorkOrderId);
            if (workOrder == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.workOrder.notFound"]);

            var branch = await _branchRepository.FindByIdAsync(quote.BranchId);
            if (branch == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.branch.notFound"]);

            var workshop = await _workshopRepository.FindByIdAsync(branch.WorkshopId.Value);
            if (workshop == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.workshop.notFound"]);

            var items = workOrder.Tasks.Select(t => new FacthubItem 
            { 
                Description = t.Description.Value, 
                Quantity = 1, 
                UnitPrice = t.Price.Amount 
            }).ToList();

            // 2. Issue Invoice via Facthub
            var request = new FacthubIssueRequest
            {
                IssuerRuc = workshop.TaxId,
                DocumentType = command.Type,
                CustomerDocumentType = command.CustomerDocumentType,
                CustomerDocumentNumber = command.CustomerDocumentNumber,
                CustomerName = command.CustomerName,
                Items = items
            };

            var response = await _facthubService.IssueInvoiceAsync(request);

            if (response == null || !response.Success || response.Invoice == null)
            {
                return Result<Voucher>.Failure(BillingErrorCodes.FacthubServiceUnavailable, _localizer["billing.error.facthub.unavailable"]);
            }

            // 3. Create Voucher
            Guid? externalInvoiceId = Guid.TryParse(response.Invoice.Id, out var parsedId) ? parsedId : null;
            var voucher = new Voucher(
                command.QuoteId,
                quote.BranchId,
                response.Invoice.Number,
                quote.SubtotalAmount,
                command.Type,
                "PEN",
                command.CustomerDocumentType,
                command.CustomerDocumentNumber,
                command.CustomerName,
                externalInvoiceId
            );

            // 4. Register full payment immediately
            voucher.AddPayment(voucher.TotalAmount, command.Method, voucher.Currency);

            await _voucherRepository.AddAsync(voucher);
            await _unitOfWork.CompleteAsync();

            return Result<Voucher>.Success(voucher);
        }
        catch (Exception ex)
        {
            var innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
            return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, _localizer["billing.error.internal"]);
        }
    }
}
