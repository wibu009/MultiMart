using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Multitenancy.Deactivate;

public class DeactivateTenantRequestValidator : CustomValidator<DeactivateTenantRequest>
{
    public DeactivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}