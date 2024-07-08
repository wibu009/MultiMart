using MultiMart.Application.Catalog.Brand.Requests.Commands;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Persistence;
using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.Brand.Validations;

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator(IReadRepository<Domain.Catalog.Brand> repository, IStringLocalizer<CreateBrandRequestValidator> T) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.FirstOrDefaultAsync(new BrandByNameSpec(name), ct) is null)
            .WithMessage((_, name) => T["Brand {0} already Exists.", name]);
}