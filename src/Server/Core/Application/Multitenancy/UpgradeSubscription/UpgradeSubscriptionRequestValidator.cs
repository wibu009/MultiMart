using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Multitenancy.UpgradeSubscription;

public class UpgradeSubscriptionRequestValidator : CustomValidator<UpgradeSubscriptionRequest>
{
    public UpgradeSubscriptionRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}