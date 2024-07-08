using MultiMart.Application.Common.Validation;
using MultiMart.Application.Multitenancy.Requests.Commands;

namespace MultiMart.Application.Multitenancy.Validations;

public class ActivateTenantRequestValidator : CustomValidator<ActivateTenantRequest>
{
    public ActivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}