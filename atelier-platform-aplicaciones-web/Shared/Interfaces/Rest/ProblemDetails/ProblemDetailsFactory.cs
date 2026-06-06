using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using atelier_platform_aplicaciones_web.Shared.Resources;

namespace atelier_platform_aplicaciones_web.Shared.Interfaces.Rest.ProblemDetails;

public class ProblemDetailsFactory(
    IStringLocalizer<SharedResource> localizer,
    Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory aspNetCoreProblemDetailsFactory)
{
    public IActionResult CreateProblemDetails(
        ControllerBase controller,
        int statusCode,
        string errorCode,
        string title = "Bad Request") 
    {
        var problemDetails = aspNetCoreProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode,
            title,
            detail: localizer[errorCode].Value,
            instance: controller.HttpContext.Request.Path
        );

        return controller.StatusCode(statusCode, problemDetails);
    }
}
