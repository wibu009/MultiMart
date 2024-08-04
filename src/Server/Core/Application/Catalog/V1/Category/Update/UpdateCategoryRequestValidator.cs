using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.V1.Category.Update;

public class UpdateCategoryRequestValidator : CustomValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator(
        IReadRepository<Domain.Catalog.Category> repository,
        IStringLocalizer<UpdateCategoryRequestValidator> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(t["Id is required."])
            .MustAsync(async (id, token) => await repository
                .AnyAsync(LambdaSpecification<Domain.Catalog.Category>.Create(spec => spec.Query.Where(c => c.Id == id)), token))
            .WithMessage(t["Category not found."]);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(t["Name is required."])
            .MaximumLength(255)
            .WithMessage(t["Name must not exceed 255 characters."])
            .MustAsync(async (request, name, token) => !await repository
                .AnyAsync(LambdaSpecification<Domain.Catalog.Category>.Create(spec => spec.Query.Where(c => c.Name == name && c.Id != request.Id)), token))
            .WithMessage((_, name) => t["Category {0} already exists.", name]);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage(t["Description must not exceed 2000 characters."]);

        RuleFor(x => x.ParentId)
            .MustAsync(async (id, token) => id is null || !await repository.AnyAsync(LambdaSpecification<Domain.Catalog.Category>.Create(spec => spec.Query.Where(c => c.Id == id)), token))
            .WithMessage(t["Parent category does not exist."]);
    }
}