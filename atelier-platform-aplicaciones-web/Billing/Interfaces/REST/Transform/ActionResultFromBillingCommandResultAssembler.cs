using System;
using atelier_platform_aplicaciones_web.Billing.Application.Internal.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Transform;

public static class ActionResultFromBillingCommandResultAssembler
{
    public static ActionResult MapFailureToActionResult(
        Enum? error, 
        string message,
        ControllerBase controller, 
        IStringLocalizer<BillingMessages> localizer)
    {
        if (error is BillingErrorCodes billingError)
        {
            var translatedMessage = localizer[message].Value ?? message;
            
            return billingError switch
            {
                BillingErrorCodes.QuoteNotFound or
                BillingErrorCodes.VoucherNotFound or
                BillingErrorCodes.PaymentNotFound =>
                    controller.Problem(
                        statusCode: 404,
                        title: "Not Found",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                        
                BillingErrorCodes.QuoteNotApproved or
                BillingErrorCodes.PaymentConflict =>
                    controller.Problem(
                        statusCode: 409,
                        title: "Conflict",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                        
                BillingErrorCodes.BadRequest or
                BillingErrorCodes.CreationFailed or
                BillingErrorCodes.UpdateFailed or
                BillingErrorCodes.ApprovalFailed or
                BillingErrorCodes.CancellationFailed or
                BillingErrorCodes.VoucherGenerationFailed =>
                    controller.Problem(
                        statusCode: 400,
                        title: "Bad Request",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                        
                BillingErrorCodes.FacthubServiceUnavailable =>
                    controller.Problem(
                        statusCode: 503,
                        title: "Service Unavailable",
                        detail: translatedMessage,
                        instance: controller.Request.Path),
                        
                _ => UnexpectedErrorResult(controller, localizer)
            };
        }

        return UnexpectedErrorResult(controller, localizer);
    }

    private static ActionResult UnexpectedErrorResult(ControllerBase controller, IStringLocalizer<BillingMessages> localizer) =>
        controller.Problem(
            title: "Internal Server Error",
            detail: localizer["billing.error.unexpected"].Value ?? "An unexpected error occurred.",
            statusCode: 500);
}
