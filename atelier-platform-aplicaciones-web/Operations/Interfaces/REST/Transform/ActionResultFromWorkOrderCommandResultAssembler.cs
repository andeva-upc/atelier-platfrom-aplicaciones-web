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

public static class ActionResultFromWorkOrderCommandResultAssembler
{
    public static ActionResult ToCreatedAtActionResult(
        Result<WorkOrder> result,
        ControllerBase controller,
        IStringLocalizer<OperationsMessages> localizer,
        string getActionName)
    {
        if (result.IsSuccess)
        {
            return controller.CreatedAtAction(
                getActionName, 
                new { id = result.Value!.Id },
                WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
        }

        return MapFailureToActionResult(result.Error, result.Message, controller, localizer);
    }

    public static ActionResult ToOkActionResult(
        Result<WorkOrder> result,
        ControllerBase controller,
        IStringLocalizer<OperationsMessages> localizer)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
        }

        return MapFailureToActionResult(result.Error, result.Message, controller, localizer);
    }

    public static ActionResult ToNoContentActionResult(
        Result<WorkOrder> result,
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
            return workOrderError switch
            {
                WorkOrderError.NotFound =>
                    controller.Problem(
                        statusCode: 404,
                        title: "Not Found",
                        detail: message,
                        instance: controller.Request.Path),
                WorkOrderError.Duplicate =>
                    controller.Problem(
                        statusCode: 409,
                        title: "Conflict",
                        detail: message,
                        instance: controller.Request.Path),
                WorkOrderError.InvalidState =>
                    controller.Problem(
                        statusCode: 400,
                        title: "Bad Request",
                        detail: message,
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
