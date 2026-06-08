using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model;
using atelier_platform_aplicaciones_web.Core.Resources;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class ActionResultFromCoreCommandResultAssembler
{
    public static ActionResult ToCreatedAtActionResult<TEntity, TResource>(
        Result<TEntity> result,
        Func<TEntity, TResource> mapper,
        ControllerBase controller,
        IStringLocalizer<CoreMessages> localizer,
        string getActionName,
        string idRouteParamName = "id")
    {
        if (result.IsSuccess)
        {
            var value = result.Value!;
            dynamic? entity = value;
            var id = entity?.Id?.Value;
            var routeValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary { { idRouteParamName, id } };
            return controller.CreatedAtAction(
                getActionName,
                routeValues,
                mapper(value));
        }

        return MapFailureToActionResult(result.Error, result.Message, controller, localizer);
    }

    public static ActionResult ToCreatedActionResult<TEntity, TResource>(
        Result<TEntity> result,
        Func<TEntity, TResource> mapper,
        ControllerBase controller,
        IStringLocalizer<CoreMessages> localizer)
    {
        if (result.IsSuccess)
        {
            return controller.StatusCode(201, mapper(result.Value!));
        }

        return MapFailureToActionResult(result.Error, result.Message, controller, localizer);
    }

    public static ActionResult ToOkActionResult<TEntity, TResource>(
        Result<TEntity> result,
        Func<TEntity, TResource> mapper,
        ControllerBase controller,
        IStringLocalizer<CoreMessages> localizer)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(mapper(result.Value!));
        }

        return MapFailureToActionResult(result.Error, result.Message, controller, localizer);
    }

    public static ActionResult ToNoContentActionResult(
        Result result,
        ControllerBase controller,
        IStringLocalizer<CoreMessages> localizer)
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
        IStringLocalizer<CoreMessages> localizer)
    {
        if (error is CoreError coreError)
        {
            var translatedMessage = localizer[message].Value ?? message;
            return coreError switch
            {
                CoreError.WorkshopNotFound or
                CoreError.BranchNotFound or
                CoreError.CustomerNotFound or
                CoreError.EmployeeNotFound or
                CoreError.OwnerNotFound or
                CoreError.SubscriptionPlanNotFound =>
                    controller.Problem(
                        statusCode: 404,
                        title: "Not Found",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                CoreError.BranchCodeMustBeUnique or
                CoreError.CustomerProfileAlreadyExists or
                CoreError.EmployeeProfileAlreadyExists or
                CoreError.OwnerProfileAlreadyExists =>
                    controller.Problem(
                        statusCode: 409,
                        title: "Conflict",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                CoreError.BranchNoActiveSubscription =>
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

    private static ActionResult UnexpectedErrorResult(ControllerBase controller, IStringLocalizer<CoreMessages> localizer) =>
        controller.Problem(
            title: localizer["core.error.unexpected"].Value,
            detail: localizer["core.error.unexpected"].Value,
            statusCode: 500);
}
