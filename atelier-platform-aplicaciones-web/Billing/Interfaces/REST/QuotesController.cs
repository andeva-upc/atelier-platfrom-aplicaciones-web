using System.Net.Mime;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class QuotesController : ControllerBase
{
    private readonly IQuoteCommandService _quoteCommandService;

    public QuotesController(IQuoteCommandService quoteCommandService)
    {
        _quoteCommandService = quoteCommandService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuote([FromBody] CreateQuoteResource resource)
    {
        var command = CreateQuoteCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _quoteCommandService.Handle(command);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        var quoteResource = QuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value);
        
        return StatusCode(201, quoteResource);
    }
}
