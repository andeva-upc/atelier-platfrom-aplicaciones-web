using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class VouchersController : ControllerBase
{
    private readonly IVoucherCommandService _voucherCommandService;

    public VouchersController(IVoucherCommandService voucherCommandService)
    {
        _voucherCommandService = voucherCommandService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Generate a voucher", Description = "Generates a new voucher for a quote adding 18% tax")]
    public async Task<IActionResult> GenerateVoucher([FromBody] GenerateVoucherResource resource)
    {
        var generateVoucherCommand = GenerateVoucherCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _voucherCommandService.Handle(generateVoucherCommand);

        if (!result.IsSuccess)
        {
            var errorResponse = new { code = result.Error?.ToString() ?? "BAD_REQUEST", message = result.Message, details = (string?)null };

            if (result.Error?.ToString() == "QuoteNotApproved")
            {
                errorResponse = new { code = "QUOTE_CONFLICT", message = "Quote must be APPROVED to generate a voucher", details = (string?)null };
                return Conflict(errorResponse);
            }

            if (result.Error?.ToString() == "FacthubServiceUnavailable")
            {
                return StatusCode(503, errorResponse);
            }

            return BadRequest(errorResponse);
        }

        var voucherResource = VoucherResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return CreatedAtAction(nameof(GenerateVoucher), new { id = voucherResource.Id }, voucherResource);
    }
}
