using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Multitenancy.Get;

public class GetTenantRequestValidator : CustomValidator<GetTenantRequest>
{
    public GetTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}