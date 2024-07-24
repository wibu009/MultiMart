using MultiMart.Application.Catalog.Category.Requests;
using MultiMart.Application.Catalog.Category.Specifications;
using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.Category.Validations;

public class CreateCategoryRequestValidation : CustomValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidation(
        IReadRepository<Domain.Catalog.Category> repository,
        IStringLocalizer<CreateCategoryRequestValidation> t)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(t["Name is required."])
            .MaximumLength(255)
            .WithMessage(t["Name must not exceed 255 characters."])
            .MustAsync(async (name, token) => await repository.FirstOrDefaultAsync(new GetCategoryByNameSpec(name), token) is null)
            .WithMessage((_, name) => t["Category {0} already exists.", name]);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage(t["Description must not exceed 2000 characters."]);

        RuleFor(x => x.ParentId)
            .MustAsync(async (id, token) => id is null || await repository.AnyAsync(new GetCategoryByIdSpec(id.Value), token))
            .WithMessage(t["Parent category does not exist."]);
    }
}