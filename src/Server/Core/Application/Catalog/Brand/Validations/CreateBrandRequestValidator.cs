using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Persistence;
using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Catalog.Brand.Validations;

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator(IReadRepository<Domain.Catalog.Brand> repository, IStringLocalizer<CreateBrandRequestValidator> t)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage(t["Brand Name is required."])
            .MaximumLength(75)
            .WithMessage(t["Brand Name is too long."])
            .MustAsync(async (name, ct) => await repository.FirstOrDefaultAsync(new GetBrandByNameSpec(name), ct) is null)
            .WithMessage((_, name) => t["Brand {0} with the same name already exists.", name]);

        RuleFor(p => p.Description)
            .MaximumLength(2000)
            .WithMessage(t["Description is too long."]);

        RuleFor(p => p.WebsiteUrl)
            .MaximumLength(500)
            .When(p => !string.IsNullOrWhiteSpace(p.WebsiteUrl))
            .WithMessage(t["Website URL is not valid."]);

        RuleFor(p => p.Email)
            .MaximumLength(500)
            .EmailAddress()
            .When(p => !string.IsNullOrWhiteSpace(p.Email))
            .WithMessage(t["Email is not valid."]);

        RuleFor(p => p.PhoneNumber)
            .MaximumLength(20)
            .Matches(@"\+?[1-9]\d{1,14}|\(?\d{1,4}\)?[\s.-]?\d{1,4}[\s.-]?\d{1,4}[\s.-]?\d{1,9}")
            .When(p => !string.IsNullOrWhiteSpace(p.PhoneNumber))
            .WithMessage(t["Phone number is not valid."]);
    }
}