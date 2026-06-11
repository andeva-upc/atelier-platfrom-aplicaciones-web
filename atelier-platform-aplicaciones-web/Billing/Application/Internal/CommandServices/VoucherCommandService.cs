using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Billing.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Billing.Application.Internal.CommandServices;

public class VoucherCommandService : IVoucherCommandService
{
    private readonly IVoucherRepository _voucherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VoucherCommandService(IVoucherRepository voucherRepository, IUnitOfWork unitOfWork)
    {
        _voucherRepository = voucherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Voucher>> Handle(GenerateVoucherCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Simulate generating a unique voucher number (in a real scenario, this might come from a sequence in DB)
            var random = new Random();
            int voucherNumber = random.Next(100000, 999999);

            var voucher = new Voucher(
                command.QuoteId,
                command.BranchId,
                voucherNumber,
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
