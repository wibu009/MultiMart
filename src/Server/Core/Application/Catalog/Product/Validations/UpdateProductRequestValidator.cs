using MultiMart.Application.Catalog.Product.Requests.Commands;
using MultiMart.Application.Catalog.Product.Specifications;
using MultiMart.Application.Common.Persistence;
using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.Product.Validations;

public class UpdateProductRequestValidator : CustomValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator(IReadRepository<Domain.Catalog.Product> productRepo, IReadRepository<Domain.Catalog.Brand> brandRepo, IStringLocalizer<UpdateProductRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (product, name, ct) =>
                    await productRepo.FirstOrDefaultAsync(new ProductByNameSpec(name), ct)
                        is not { } existingProduct || existingProduct.Id == product.Id)
                .WithMessage((_, name) => T["Product {0} already Exists.", name]);

        RuleFor(p => p.Rate)
            .GreaterThanOrEqualTo(1);

        RuleFor(p => p.Image);

        RuleFor(p => p.BrandId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await brandRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => T["Brand {0} Not Found.", id]);
    }
}