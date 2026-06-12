using atelier_platform_aplicaciones_web.Shared.Domain.Model;
using atelier_platform_aplicaciones_web.Operations.Domain.Model;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Operations.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;

public static class ActionResultFromServiceCommandResultAssembler
{
    public static ActionResult ToCreatedAtActionResult(
        Result<Service> result,
        ControllerBase controller,
        IStringLocalizer<OperationsMessages> localizer,
        string getActionName)
    {
        if (result.IsSuccess)
        {
            return controller.CreatedAtAction(
                getActionName, 
                new { serviceId = result.Value!.Id.Value },
                ServiceResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
        }

        return MapFailureToActionResult(result.Error, result.Message, controller, localizer);
    }

    public static ActionResult ToOkActionResult(
        Result<Service> result,
        ControllerBase controller,
        IStringLocalizer<OperationsMessages> localizer)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(ServiceResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
        }

        return MapFailureToActionResult(result.Error, result.Message, controller, localizer);
    }

    public static ActionResult ToNoContentActionResult(
        Result result,
        ControllerBase controller,
        IStringLocalizer<OperationsMessages> localizer)
    {
        if (result.IsSuccess)
        {
            return controller.NoContent();
        }

        return MapFailureToActionResult(result.Error, result.Message, controller, localizer);
    }

    private static ActionResult MapFailureToActionResult(
        Enum? error, 
        string message,
        ControllerBase controller, 
        IStringLocalizer<OperationsMessages> localizer)
    {
        if (error is WorkOrderError workOrderError)
        {
            var translatedMessage = localizer[message].Value ?? message;
            return workOrderError switch
            {
                WorkOrderError.NotFound =>
                    controller.Problem(
                        statusCode: 404,
                        title: "Not Found",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                WorkOrderError.Duplicate =>
                    controller.Problem(
                        statusCode: 409,
                        title: "Conflict",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                WorkOrderError.InvalidState =>
                    controller.Problem(
                        statusCode: 400,
                        title: "Bad Request",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                _ => UnexpectedErrorResult(controller, localizer)
            };
        }

        return UnexpectedErrorResult(controller, localizer);
    }

    private static ActionResult UnexpectedErrorResult(ControllerBase controller, IStringLocalizer<OperationsMessages> localizer) =>
        controller.Problem(
            title: localizer["operations.error.unexpected"].Value,
            detail: localizer["operations.error.unexpected"].Value,
            statusCode: 500);
}
