using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.V1.Brand.Update;

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator(
        IReadRepository<Domain.Catalog.Brand> repository,
        IStringLocalizer<UpdateBrandRequestValidator> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(t["Id is required"])
            .MustAsync(async (id, cancellationToken) => await repository
                .AnyAsync(
                    LambdaSpecification<Domain.Catalog.Brand>.Create(spec => spec.Query.Where(b => b.Id == id)), cancellationToken))
            .WithMessage(t["Brand not found"]);

        RuleFor(x => x.Name)
            .MaximumLength(255)
            .WithMessage(t["Name is too long"])
            .MustAsync(async (request, name, cancellationToken) => await repository
                .FirstOrDefaultAsync(
                    LambdaSingleResultSpecification<Domain.Catalog.Brand>.Create(spec => spec.Query.Where(b => b.Name == name && b.Id != request.Id)), cancellationToken) == null)
            .WithMessage((_, name) => t["Brand with {0} already exists.", name]);

        RuleFor(x => x.Description)
            .MaximumLength(4000)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage(t["Description is too long"]);
    }
}