﻿using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Persistence;
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

        RuleFor(x => x.WebsiteUrl)
            .MaximumLength(255)
            .When(x => !string.IsNullOrWhiteSpace(x.WebsiteUrl))
            .WithMessage(t["Website URL is too long"]);

        RuleFor(x => x.Email)
            .MaximumLength(255)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage(t["Email is not valid"]);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(255)
            .Matches(@"\+?[1-9]\d{1,14}|\(?\d{1,4}\)?[\s.-]?\d{1,4}[\s.-]?\d{1,4}[\s.-]?\d{1,9}")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage(t["Phone number is not valid"]);
    }
}