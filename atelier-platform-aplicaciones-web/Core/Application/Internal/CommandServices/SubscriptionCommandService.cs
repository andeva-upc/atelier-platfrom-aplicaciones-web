using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.CommandServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace atelier_platform_aplicaciones_web.Core.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    IBranchSubscriptionRepository subscriptionRepository,
    ISubscriptionPlanRepository planRepository,
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork,
    ILogger<SubscriptionCommandService> logger) : ISubscriptionCommandService
{
    public async Task<Result<BranchSubscription>> Handle(AssignSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        if (!await branchRepository.ExistsByIdAsync(command.BranchId, cancellationToken))
        {
            return Result<BranchSubscription>.Failure(CoreError.BranchNotFound, "core.error.branch.notFound");
        }

        var plan = await planRepository.FindSubscriptionPlanByIdAsync(command.PlanId, cancellationToken);
        if (plan == null)
        {
            return Result<BranchSubscription>.Failure(CoreError.SubscriptionPlanNotFound, "Subscription plan does not exist.");
        }

        // Mock Payment Processing
        if (command.CreditCard != null && !string.IsNullOrWhiteSpace(command.CreditCard.CardNumber))
        {
            var len = command.CreditCard.CardNumber.Length;
            var suffix = command.CreditCard.CardNumber.Substring(Math.Max(0, len - 4));
            logger.LogInformation("Simulating payment processing via Stripe for card ending in {Suffix}", suffix);
            logger.LogInformation("Payment successful for Branch ID: {BranchId}", command.BranchId.Value);
        }

        var existingSubscription = await subscriptionRepository.FindActiveByBranchIdAsync(command.BranchId, cancellationToken);
        if (existingSubscription != null)
        {
            existingSubscription.Cancel(DateTime.UtcNow);
            subscriptionRepository.Update(existingSubscription);
        }

        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddMonths(command.BillingCycle == BillingCycle.Monthly ? 1 : 12);

        var newSubscription = new BranchSubscription(
            command.BranchId,
            command.PlanId,
            command.BillingCycle,
            startDate,
            endDate
        );

        await subscriptionRepository.AddAsync(newSubscription, cancellationToken);
        await unitOfWork.CompleteAsync();

        return Result<BranchSubscription>.Success(newSubscription);
    }

    public async Task<Result<BranchSubscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        var existingSubscription = await subscriptionRepository.FindActiveByBranchIdAsync(command.BranchId, cancellationToken);
        if (existingSubscription == null)
        {
            return Result<BranchSubscription>.Failure(CoreError.BranchNoActiveSubscription, "core.error.branch.noActiveSubscription");
        }

        existingSubscription.Cancel(DateTime.UtcNow);
        subscriptionRepository.Update(existingSubscription);
        await unitOfWork.CompleteAsync();

        return Result<BranchSubscription>.Success(existingSubscription);
    }
}
