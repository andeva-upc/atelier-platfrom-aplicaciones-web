using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Billing.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class VouchersController : ControllerBase
{
    private readonly IVoucherCommandService _voucherCommandService;
    private readonly atelier_platform_aplicaciones_web.Billing.Application.QueryServices.IVoucherQueryService _voucherQueryService;
    private readonly IStringLocalizer<BillingMessages> _localizer;

    public VouchersController(IVoucherCommandService voucherCommandService, atelier_platform_aplicaciones_web.Billing.Application.QueryServices.IVoucherQueryService voucherQueryService, IStringLocalizer<BillingMessages> localizer)
    {
        _voucherCommandService = voucherCommandService;
        _voucherQueryService = voucherQueryService;
        _localizer = localizer;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Generate a voucher", Description = "Generates a new voucher for a quote adding 18% tax")]
    public async Task<IActionResult> GenerateVoucher([FromBody] GenerateVoucherResource resource)
    {
        var generateVoucherCommand = GenerateVoucherCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _voucherCommandService.Handle(generateVoucherCommand);

        if (result.IsSuccess)
        {
            var voucherResource = VoucherResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
            return CreatedAtAction(nameof(GetVoucherById), new { id = voucherResource.Id }, voucherResource);
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get voucher by id", Description = "Gets a voucher by its identifier")]
    public async Task<IActionResult> GetVoucherById(System.Guid id)
    {
        var query = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries.GetVoucherByIdQuery(id);
        var voucher = await _voucherQueryService.Handle(query);

        if (voucher == null)
            return NotFound();

        var resource = VoucherResourceFromEntityAssembler.ToResourceFromEntity(voucher);
        return Ok(resource);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get vouchers by branch id", Description = "Gets all vouchers for a specific branch")]
    public async Task<IActionResult> GetVouchersByBranchId([FromQuery] System.Guid branchId)
    {
        if (branchId == System.Guid.Empty)
            return BadRequest(new { code = "BAD_REQUEST", message = "branchId is required", details = (string?)null });

        var query = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries.GetVouchersByBranchIdQuery(branchId);
        var vouchers = await _voucherQueryService.Handle(query);

        var resources = System.Linq.Enumerable.Select(vouchers, VoucherResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost("{voucherId}/payments")]
    [SwaggerOperation(Summary = "Add a payment to a voucher", Description = "Adds a partial or full payment to a voucher")]
    public async Task<IActionResult> AddPayment(System.Guid voucherId, [FromBody] AddPaymentResource resource)
    {
        var command = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands.AddPaymentCommand(voucherId, resource.Amount, resource.Method);
        var result = await _voucherCommandService.Handle(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Payment registered successfully", paymentId = result.Value?.Id });
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }

    [HttpDelete("{voucherId}/payments/{paymentId}")]
    [SwaggerOperation(Summary = "Remove a payment from a voucher", Description = "Removes a payment registered by mistake")]
    public async Task<IActionResult> DeletePayment(System.Guid voucherId, System.Guid paymentId)
    {
        var command = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands.RemovePaymentCommand(voucherId, paymentId);
        var result = await _voucherCommandService.Handle(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Payment removed successfully" });
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }

    [HttpPost("checkout")]
    [SwaggerOperation(Summary = "Process complete checkout", Description = "Generates a voucher and registers the total payment immediately")]
    public async Task<IActionResult> ProcessCheckout([FromBody] ProcessCheckoutResource resource)
    {
        var command = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands.ProcessCheckoutCommand(
            resource.QuoteId,
            resource.Type,
            resource.CustomerDocumentType,
            resource.CustomerDocumentNumber,
            resource.CustomerName,
            resource.Method
        );

        var result = await _voucherCommandService.Handle(command);

        if (result.IsSuccess)
        {
            var voucherResource = VoucherResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
            return CreatedAtAction(nameof(GetVoucherById), new { id = voucherResource.Id }, voucherResource);
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }
}
