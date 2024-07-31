using MultiMart.Application.Catalog.Category.Requests;
using MultiMart.Application.Catalog.Category.Specifications;
using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.Category.Validations;

public class UpdateCategoryRequestValidation : CustomValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidation(
        IReadRepository<Domain.Catalog.Category> repository,
        IStringLocalizer<UpdateCategoryRequestValidation> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(t["Id is required."])
            .MustAsync(async (id, token) => await repository.AnyAsync(new GetCategoryByIdSpec(id), token))
            .WithMessage(t["Category not found."]);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(t["Name is required."])
            .MaximumLength(255)
            .WithMessage(t["Name must not exceed 255 characters."])
            .MustAsync(async (request, name, token) => await repository.FirstOrDefaultAsync(new GetCategoryByNameSpec(name, request.Id), token) is null)
            .WithMessage((_, name) => t["Category {0} already exists.", name]);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage(t["Description must not exceed 2000 characters."]);

        RuleFor(x => x.ParentId)
            .MustAsync(async (id, token) => id is null || await repository.AnyAsync(new GetCategoryByIdSpec(id.Value), token))
            .WithMessage(t["Parent category does not exist."]);
    }
}