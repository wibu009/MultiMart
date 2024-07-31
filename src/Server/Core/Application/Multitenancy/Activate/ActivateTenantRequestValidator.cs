using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Multitenancy.Activate;

public class ActivateTenantRequestValidator : CustomValidator<ActivateTenantRequest>
{
    public ActivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}