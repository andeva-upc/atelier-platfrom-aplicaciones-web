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

    public VoucherCommandService(
        IVoucherRepository voucherRepository, 
        IQuoteRepository quoteRepository,
        IWorkOrderRepository workOrderRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IWorkshopRepository workshopRepository,
        IFacthubService facthubService,
        IUnitOfWork unitOfWork)
    {
        _voucherRepository = voucherRepository;
        _quoteRepository = quoteRepository;
        _workOrderRepository = workOrderRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _workshopRepository = workshopRepository;
        _facthubService = facthubService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Voucher>> Handle(GenerateVoucherCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Gather all required data for Facthub
            var quote = await _quoteRepository.FindByIdAsync(command.QuoteId);
            if (quote == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, "Quote not found.");

            var workOrder = await _workOrderRepository.FindByIdAsync(quote.WorkOrderId);
            if (workOrder == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, "WorkOrder not found.");

            var customer = await _customerRepository.FindByIdAsync(workOrder.CustomerId.Value);
            if (customer == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, "Customer not found.");

            var branch = await _branchRepository.FindByIdAsync(command.BranchId);
            if (branch == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, "Branch not found.");

            var workshop = await _workshopRepository.FindByIdAsync(branch.WorkshopId.Value);
            if (workshop == null) return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, "Workshop not found.");

            // 2. Prepare Facthub Request
            var request = new FacthubIssueRequest
            {
                IssuerRuc = workshop.TaxId,
                DocumentType = command.Type,
                CustomerDocumentType = customer.Document.DocumentType.ToString(),
                CustomerDocumentNumber = customer.Document.DocumentNumber,
                CustomerName = !string.IsNullOrEmpty(customer.BusinessName) ? customer.BusinessName : (customer.Name?.FullName ?? string.Empty),
                Items = command.Items.Select(i => new FacthubItem 
                { 
                    Description = i.Description, 
                    Quantity = i.Quantity, 
                    UnitPrice = i.UnitPrice 
                }).ToList()
            };

            // 3. Call Facthub Service
            var response = await _facthubService.IssueInvoiceAsync(request);

            if (response == null || !response.Success || response.Invoice == null)
            {
                return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, "Failed to issue invoice in Facthub: " + (response?.Message ?? "Unknown error"));
            }

            // 4. Create Voucher with Facthub data
            var voucher = new Voucher(
                command.QuoteId,
                command.BranchId,
                response.Invoice.Series,
                response.Invoice.Number,
                command.SubtotalAmount,
                command.Type,
                command.Currency
            );

            await _voucherRepository.AddAsync(voucher);
            await _unitOfWork.CompleteAsync();

            return Result<Voucher>.Success(voucher);
        }
        catch (Exception ex)
        {
            return Result<Voucher>.Failure(BillingErrorCodes.VoucherGenerationFailed, $"An error occurred while generating the voucher: {ex.Message}");
        }
    }
}
