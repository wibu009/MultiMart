using MultiMart.Application.Common.Validation;
using MultiMart.Application.Multitenancy.Requests.Commands;

namespace MultiMart.Application.Multitenancy.Validations;

public class UpgradeSubscriptionRequestValidator : CustomValidator<UpgradeSubscriptionRequest>
{
    public UpgradeSubscriptionRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}