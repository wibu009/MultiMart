using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.V1.Brand.Get;

public class GetBrandRequestValidator : CustomValidator<GetBrandRequest>
{
    public GetBrandRequestValidator(
        IReadRepository<Domain.Catalog.Brand> repository,
        IStringLocalizer<GetBrandRequestValidator> t)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(t["Id is required."])
            .MustAsync(async (id, cancellationToken)
                => await repository
                    .AnyAsync(
                        LambdaSingleResultSpecification<Domain.Catalog.Brand>.Create(spec => spec.Query.Where(b => b.Id == id)), cancellationToken))
            .WithMessage(t["Brand not found."]);
    }
}