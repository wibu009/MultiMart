using MultiMart.Application.Catalog.Brand.Requests.Commands;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Persistence;
using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.Brand.Validations;

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator(IRepository<Domain.Catalog.Brand> repository, IStringLocalizer<UpdateBrandRequestValidator> T) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (brand, name, ct) =>
                await repository.FirstOrDefaultAsync(new BrandByNameSpec(name), ct)
                    is not { } existingBrand || existingBrand.Id == brand.Id)
            .WithMessage((_, name) => T["Brand {0} already Exists.", name]);
}