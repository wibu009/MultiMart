using MultiMart.Application.Catalog.Category.Requests;
using MultiMart.Application.Catalog.Category.Specifications;
using MultiMart.Application.Common.Validation;
using MultiMart.Domain.Catalog.Product;

namespace MultiMart.Application.Catalog.Category.Validations;

public class DeleteCategoryRequestValidation : CustomValidator<DeleteCategoryRequest>
{
    public DeleteCategoryRequestValidation(
        IReadRepository<Domain.Catalog.Category> categoryRepository,
        IReadRepository<Product> productRepository,
        IStringLocalizer<DeleteCategoryRequestValidation> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(t["Id is required."])
            .MustAsync(async (id, cancellationToken)
                => !await categoryRepository.AnyAsync(new GetCategoryByIdSpec(id), cancellationToken))
            .WithMessage(t["Category not found."])
            .MustAsync(async (id, cancellationToken)
                => await productRepository.AnyAsync(new GetProductByCategorySpec(id), cancellationToken))
            .WithMessage(t["Category has products, cannot delete."]);
    }
}