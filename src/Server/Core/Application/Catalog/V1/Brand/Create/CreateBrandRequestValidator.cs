using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.V1.Brand.Create;

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator(
        IReadRepository<Domain.Catalog.Brand> repository,
        IStringLocalizer<CreateBrandRequestValidator> t)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage(t["Brand Name is required."])
            .MaximumLength(75)
            .WithMessage(t["Brand Name is too long."])
            .MustAsync(async (name, ct)
                => await repository
                    .FirstOrDefaultAsync(
                        LambdaSingleResultSpecification<Domain.Catalog.Brand>.Create(spec => spec.Query.Where(b => b.Name == name)), ct) is null)
            .WithMessage((_, name) => t["Brand {0} with the same name already exists.", name]);

        RuleFor(p => p.Description)
            .MaximumLength(2000)
            .WithMessage(t["Description is too long."]);
    }
}