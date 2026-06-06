using atelier_platform_aplicaciones_web.Operations.Application.Errors;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;

public static class ActionResultFromWorkOrderCommandResultAssembler
{
    // Mapea a 201 Created (usado en la creación)
    public static ActionResult ToCreatedAtActionResult(
        Result<WorkOrder, WorkOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getActionName) =>
        result switch
        {
            Result<WorkOrder, WorkOrderError>.Success success =>
                controller.CreatedAtAction(
                    getActionName, 
                    new { id = success.Value.Id },
                    WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<WorkOrder, WorkOrderError>.Failure failure =>
                MapFailureToActionResult(failure.Error, controller, localizer),

            _ => UnexpectedErrorResult(controller, localizer)
        };

    // Mapea a 200 OK (usado en actualizaciones y modificaciones)
    public static ActionResult ToOkActionResult(
        Result<WorkOrder, WorkOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<WorkOrder, WorkOrderError>.Success success =>
                controller.Ok(WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<WorkOrder, WorkOrderError>.Failure failure =>
                MapFailureToActionResult(failure.Error, controller, localizer),

            _ => UnexpectedErrorResult(controller, localizer)
        };

    // Mapea a 204 No Content (usado en borrados)
    public static ActionResult ToNoContentActionResult(
        Result<WorkOrder, WorkOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<WorkOrder, WorkOrderError>.Success =>
                controller.NoContent(),

            Result<WorkOrder, WorkOrderError>.Failure failure =>
                MapFailureToActionResult(failure.Error, controller, localizer),

            _ => UnexpectedErrorResult(controller, localizer)
        };

    // Mapea las fallas del dominio a códigos de estado HTTP
    private static ActionResult MapFailureToActionResult(
        WorkOrderError error, 
        ControllerBase controller, 
        IStringLocalizer<SharedResource> localizer) =>
        error.Type switch
        {
            "NotFound" =>
                controller.Problem(
                    statusCode: 404,
                    title: "Not Found",
                    detail: localizer[error.Code].Value,
                    instance: controller.Request.Path),
            "Duplicate" =>
                controller.Problem(
                    statusCode: 409,
                    title: "Conflict",
                    detail: localizer[error.Code].Value,
                    instance: controller.Request.Path),
            "InvalidState" =>
                controller.Problem(
                    statusCode: 400,
                    title: "Bad Request",
                    detail: localizer[error.Code].Value, // Aquí se inyectará la traducción específica
                    instance: controller.Request.Path),
            _ => UnexpectedErrorResult(controller, localizer)
        };

    private static ActionResult UnexpectedErrorResult(ControllerBase controller, IStringLocalizer<SharedResource> localizer) =>
        controller.Problem(
            title: localizer["UnexpectedServerError"].Value,
            detail: localizer["UnexpectedErrorProcessingRequest"].Value,
            statusCode: 500);
}
