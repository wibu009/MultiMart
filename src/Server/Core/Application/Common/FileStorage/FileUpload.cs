using MultiMart.Application.Common.Validation;

namespace MultiMart.Application.Common.FileStorage;

public class FileUpload
{
    public string Name { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public string Data { get; set; } = default!;
}

public class FileUploadRequestValidator : CustomValidator<FileUpload>
{
    public FileUploadRequestValidator(IStringLocalizer<FileUploadRequestValidator> t)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
                .WithMessage(t["Image Name cannot be empty!"])
            .MaximumLength(150);

        RuleFor(p => p.Extension)
            .NotEmpty()
                .WithMessage(t["Image Extension cannot be empty!"])
            .MaximumLength(5);

        RuleFor(p => p.Data)
            .NotEmpty()
                .WithMessage(t["Image Data cannot be empty!"]);
    }
}