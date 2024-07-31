using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.V1.Category.Get;

public class GetCategoryRequestValidator : CustomValidator<GetCategoryRequest>
{
    public GetCategoryRequestValidator(
        IReadRepository<Domain.Catalog.Category> repository,
        IStringLocalizer<GetCategoryRequestValidator> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(t["Id is required."])
            .MustAsync(async (id, token) => await repository
                .AnyAsync(LambdaSingleResultSpecification<Domain.Catalog.Category>.Create(spec => spec.Query.Where(x => x.Id == id)), token))
            .WithMessage(t["Category not found."]);
    }
}