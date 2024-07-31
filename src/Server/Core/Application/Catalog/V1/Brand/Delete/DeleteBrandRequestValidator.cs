using MultiMart.Application.Common.Validation;
using MultiMart.Domain.Catalog.Products;

namespace MultiMart.Application.Catalog.V1.Brand.Delete;

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
                => !await brandRepository
                    .AnyAsync(
                        LambdaSingleResultSpecification<Domain.Catalog.Brand>.Create(spec => spec.Query.Where(b => b.Id == id)), cancellationToken))
            .WithMessage(t["Brand not found."])
            .MustAsync(async (id, cancellationToken)
                => await productRepository
                    .AnyAsync(
                        LambdaSingleResultSpecification<Product>.Create(spec => spec.Query.Where(x => x.BrandId == id)), cancellationToken))
            .WithMessage(t["Brand has products, cannot delete."]);
    }
}