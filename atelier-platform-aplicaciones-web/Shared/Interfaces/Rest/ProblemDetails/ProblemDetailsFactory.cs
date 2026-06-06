using Microsoft.AspNetCore.Mvc;

namespace atelier_platform_aplicaciones_web.Shared.Interfaces.Rest.ProblemDetails;

public class ProblemDetailsFactory(
    Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory aspNetCoreProblemDetailsFactory)
{
    public IActionResult CreateProblemDetails(
        ControllerBase controller,
        int statusCode,
        string errorMessage,
        string title = "Bad Request") 
    {
        var problemDetails = aspNetCoreProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode,
            title,
            detail: errorMessage,
            instance: controller.HttpContext.Request.Path
        );

        return controller.StatusCode(statusCode, problemDetails);
    }
}
