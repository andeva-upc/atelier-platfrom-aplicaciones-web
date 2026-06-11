using System.Net.Mime;
using System.Threading.Tasks;
using System;
using System.Linq;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Application.QueryServices;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Billing.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class QuotesController : ControllerBase
{
    private readonly IQuoteCommandService _quoteCommandService;
    private readonly IQuoteQueryService _quoteQueryService;
    private readonly IStringLocalizer<BillingMessages> _localizer;

    public QuotesController(IQuoteCommandService quoteCommandService, IQuoteQueryService quoteQueryService, IStringLocalizer<BillingMessages> localizer)
    {
        _quoteCommandService = quoteCommandService;
        _quoteQueryService = quoteQueryService;
        _localizer = localizer;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new quote", Description = "Creates a new draft quote for a specific work order and branch")]
    public async Task<IActionResult> CreateQuote([FromBody] CreateQuoteResource resource)
    {
        var command = CreateQuoteCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _quoteCommandService.Handle(command);

        if (result.IsSuccess)
        {
            var quoteResource = QuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
            return StatusCode(201, quoteResource);
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }

    [HttpGet("{quoteId}")]
    [SwaggerOperation(Summary = "Get a quote by ID", Description = "Retrieves the details of a specific quote")]
    public async Task<IActionResult> GetQuoteById(Guid quoteId)
    {
        var getQuoteByIdQuery = new Billing.Domain.Model.Queries.GetQuoteByIdQuery(quoteId);
        var quote = await _quoteQueryService.Handle(getQuoteByIdQuery);

        if (quote == null)
            return NotFound();

        var quoteResource = QuoteResourceFromEntityAssembler.ToResourceFromEntity(quote);
        return Ok(quoteResource);
    }

    [HttpGet("branch/{branchId}")]
    [SwaggerOperation(Summary = "Get quotes by branch ID", Description = "Retrieves all quotes associated with a specific branch")]
    public async Task<IActionResult> GetQuotesByBranchId(Guid branchId)
    {
        var getQuotesByBranchIdQuery = new Billing.Domain.Model.Queries.GetQuotesByBranchIdQuery(branchId);
        var quotes = await _quoteQueryService.Handle(getQuotesByBranchIdQuery);

        var quoteResources = quotes.Select(QuoteResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(quoteResources);
    }

    [HttpPut("{quoteId}")]
    [SwaggerOperation(Summary = "Update quote details", Description = "Updates the subtotal and discount percentage of an existing quote")]
    public async Task<IActionResult> UpdateQuote(Guid quoteId, [FromBody] UpdateQuoteResource resource)
    {
        var updateQuoteCommand = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands.UpdateQuoteCommand(
            quoteId,
            resource.SubtotalAmount,
            resource.DiscountPercentage
        );

        var result = await _quoteCommandService.Handle(updateQuoteCommand);

        if (result.IsSuccess)
        {
            var quoteResource = QuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
            return Ok(quoteResource);
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }

    [HttpPost("{quoteId}/approve")]
    [SwaggerOperation(Summary = "Approve a quote", Description = "Changes the status of a draft quote to APPROVED")]
    public async Task<IActionResult> ApproveQuote(Guid quoteId)
    {
        var approveQuoteCommand = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands.ApproveQuoteCommand(quoteId);
        var result = await _quoteCommandService.Handle(approveQuoteCommand);

        if (result.IsSuccess)
        {
            var quoteResource = QuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
            return Ok(quoteResource);
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }

    [HttpPost("{quoteId}/cancel")]
    [SwaggerOperation(Summary = "Cancel a quote", Description = "Changes the status of a quote to CANCELLED")]
    public async Task<IActionResult> CancelQuote(Guid quoteId)
    {
        var cancelQuoteCommand = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands.CancelQuoteCommand(quoteId);
        var result = await _quoteCommandService.Handle(cancelQuoteCommand);

        if (result.IsSuccess)
        {
            var quoteResource = QuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
            return Ok(quoteResource);
        }

        return ActionResultFromBillingCommandResultAssembler.MapFailureToActionResult(result.Error, result.Message, this, _localizer);
    }
}
