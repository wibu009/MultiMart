using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.V1.Category.Create;

public class CreateCategoryRequestValidator : CustomValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator(
        IReadRepository<Domain.Catalog.Category> repository,
        IStringLocalizer<CreateCategoryRequestValidator> t)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(t["Name is required."])
            .MaximumLength(255)
            .WithMessage(t["Name must not exceed 255 characters."])
            .MustAsync(async (name, token) => await repository.AnyAsync(LambdaSpecification<Domain.Catalog.Category>.Create(spec => spec.Query.Where(c => c.Name == name)), token))
            .WithMessage((_, name) => t["Category {0} already exists.", name]);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage(t["Description must not exceed 2000 characters."]);

        RuleFor(x => x.ParentId)
            .MustAsync(async (id, token) => id is null || await repository.AnyAsync(LambdaSpecification<Domain.Catalog.Category>.Create(spec => spec.Query.Where(c => c.Id == id)), token))
            .WithMessage(t["Parent category does not exist."]);
    }
}