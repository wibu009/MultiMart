using MultiMart.Application.Common.Validation;
using MultiMart.Application.Multitenancy.Requests.Commands;

namespace MultiMart.Application.Multitenancy.Validations;

public class DeactivateTenantRequestValidator : CustomValidator<DeactivateTenantRequest>
{
    public DeactivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}