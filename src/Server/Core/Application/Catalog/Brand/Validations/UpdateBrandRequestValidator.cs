using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.Brand.Validations;

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator(IReadRepository<Domain.Catalog.Brand> repository, IStringLocalizer<UpdateBrandRequestValidator> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(t["Id is required"])
            .MustAsync(async (id, cancellationToken) => await repository.AnyAsync(new GetBrandByIdSpec(id), cancellationToken))
            .WithMessage(t["Brand not found"]);

        RuleFor(x => x.Name)
            .MaximumLength(255)
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage(t["Name is too long"]);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage(t["Description is too long"]);
    }
}