using System;
using atelier_platform_aplicaciones_web.IoT.Domain.Model;
using atelier_platform_aplicaciones_web.IoT.Resources;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class ActionResultFromIoTCommandResultAssembler
{
    public static ActionResult ToCreatedAtActionResult<TEntity, TResource>(
        Result<TEntity> result,
        Func<TEntity, TResource> mapper,
        ControllerBase controller,
        IStringLocalizer<IoTMessages> localizer,
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
        IStringLocalizer<IoTMessages> localizer)
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
        IStringLocalizer<IoTMessages> localizer)
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
        IStringLocalizer<IoTMessages> localizer)
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
        IStringLocalizer<IoTMessages> localizer)
    {
        if (error is IoTError iotError)
        {
            var translatedMessage = localizer[message].Value ?? message;
            return iotError switch
            {
                IoTError.Obd2DeviceNotFound or
                IoTError.Obd2DeviceRegistrationNotFound or
                IoTError.VehicleNotFound or
                IoTError.VehicleRegistrationNotFound =>
                    controller.Problem(
                        statusCode: 404,
                        title: "Not Found",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                IoTError.DuplicateMacAddress or
                IoTError.VinAlreadyRegistered or
                IoTError.PlateNumberAlreadyRegistered or
                IoTError.Obd2DeviceAlreadyLinked =>
                    controller.Problem(
                        statusCode: 409,
                        title: "Conflict",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                _ => UnexpectedErrorResult(controller, localizer)
            };
        }

        return UnexpectedErrorResult(controller, localizer);
    }

    private static ActionResult UnexpectedErrorResult(ControllerBase controller, IStringLocalizer<IoTMessages> localizer) =>
        controller.Problem(
            title: localizer["iot.error.unexpected"].Value,
            detail: localizer["iot.error.unexpected"].Value,
            statusCode: 500);
}
