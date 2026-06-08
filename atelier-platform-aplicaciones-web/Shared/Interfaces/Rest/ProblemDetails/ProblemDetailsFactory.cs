using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace atelier_platform_aplicaciones_web.Shared.Interfaces.Rest.ProblemDetails;

public class ProblemDetailsFactory(
    Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory aspNetCoreProblemDetailsFactory,
    IStringLocalizerFactory localizerFactory)
{
    public IActionResult CreateProblemDetails(
        ControllerBase controller,
        int statusCode,
        Enum? errorEnum = null,
        string? errorMessage = null,
        string title = "Bad Request") 
    {
        var detailMessage = errorMessage;
        var problemTitle = title;

        if (errorEnum != null)
        {
            var enumType = errorEnum.GetType();
            var namespaceName = enumType.Namespace ?? "";

            Type? resourceType = null;
            if (namespaceName.Contains("IAM"))
                resourceType = typeof(atelier_platform_aplicaciones_web.IAM.Resources.IamMessages);
            else if (namespaceName.Contains("Operations"))
                resourceType = typeof(atelier_platform_aplicaciones_web.Operations.Resources.OperationsMessages);
            else
                resourceType = typeof(Resources.SharedResource);

            var localizer = localizerFactory.Create(resourceType);
            var localizedMessage = localizer[$"{errorEnum}"].Value;
            
            if (localizedMessage != $"{errorEnum}") // Valid translation found
            {
                detailMessage = localizedMessage;
            }
        }

        var problemDetails = aspNetCoreProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode,
            problemTitle,
            detail: detailMessage,
            instance: controller.HttpContext.Request.Path
        );

        return controller.StatusCode(statusCode, problemDetails);
    }
}
