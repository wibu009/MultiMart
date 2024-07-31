using MultiMart.Application.Common.Validation;
using MultiMart.Domain.Catalog.Products;

namespace MultiMart.Application.Catalog.V1.Category.Delete;

public class DeleteCategoryRequestValidator : CustomValidator<DeleteCategoryRequest>
{
    public DeleteCategoryRequestValidator(
        IReadRepository<Domain.Catalog.Category> categoryRepository,
        IReadRepository<Product> productRepository,
        IStringLocalizer<DeleteCategoryRequestValidator> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(t["Id is required."])
            .MustAsync(async (id, cancellationToken)
                => await categoryRepository.AnyAsync(
                    LambdaSpecification<Domain.Catalog.Category>.Create(spec => spec.Query.Where(c => c.Id == id)), cancellationToken))
            .WithMessage(t["Category not found."])
            .MustAsync(async (id, cancellationToken)
                => await productRepository.AnyAsync(
                    LambdaSpecification<Product>.Create(spec => spec.Query.Where(p => p.CategoryId == id)), cancellationToken))
            .WithMessage(t["Category has products, cannot delete."]);
    }
}