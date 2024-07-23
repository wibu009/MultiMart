using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Persistence;
using MultiMart.Application.Common.Validation;
using MultiMart.Domain.Catalog.Product;

namespace MultiMart.Application.Catalog.Brand.Validations;

public class DeleteBrandRequestValidator : CustomValidator<DeleteBrandRequest>
{
    public DeleteBrandRequestValidator(
        IReadRepository<Product> productReadRepository,
        IStringLocalizer<DeleteBrandRequestValidator> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required")
            .MustAsync(async (id, cancellationToken)
                => await productReadRepository.AnyAsync(new GetProductByBrandSpec(id), cancellationToken))
            .WithMessage(t["Brand has products, cannot delete."]);
    }
}