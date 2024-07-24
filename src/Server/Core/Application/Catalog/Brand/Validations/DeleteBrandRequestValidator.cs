using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Validation;
using MultiMart.Domain.Catalog.Product;

namespace MultiMart.Application.Catalog.Brand.Validations;

public class DeleteBrandRequestValidator : CustomValidator<DeleteBrandRequest>
{
    public DeleteBrandRequestValidator(
        IReadRepository<Domain.Catalog.Brand> brandRepository,
        IReadRepository<Product> productRepository,
        IStringLocalizer<DeleteBrandRequestValidator> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(t["Id is required."])
            .MustAsync(async (id, cancellationToken)
                => !await brandRepository.AnyAsync(new GetBrandByIdSpec(id), cancellationToken))
            .WithMessage(t["Brand not found."])
            .MustAsync(async (id, cancellationToken)
                => await productRepository.AnyAsync(new GetProductByBrandSpec(id), cancellationToken))
            .WithMessage(t["Brand has products, cannot delete."]);
    }
}