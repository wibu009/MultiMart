using MultiMart.Application.Common.Events;
using MultiMart.Application.Common.FileStorage;
using MultiMart.Application.Common.FileStorage.Cloudinary;
using MultiMart.Application.Common.Validation;
using MultiMart.Domain.Common.Enums;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Brand.V1;

public abstract class CreateBrandCommand
{
    public class Request : IRequest<string>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public FileUpload? Logo { get; set; }
    }

    private sealed class GetBrandByNameSpec : Specification<Domain.Catalog.Brand>, ISingleResultSpecification
    {
        public GetBrandByNameSpec(string name)
            => Query.Where(b => b.Name == name);
    }

    public class Validator : CustomValidator<Request>
    {
        public Validator(IReadRepository<Domain.Catalog.Brand> repository, IStringLocalizer<Validator> t)
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
        }
    }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Request, Domain.Catalog.Brand>()
                .Ignore(dest => dest.LogoUrl!)
                .IgnoreNullValues(true);
        }
    }

    public class Handler : IRequestHandler<Request, string>
    {
        private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
        private readonly ICloudinaryFileStorageService _cloudinaryFileStorageService;
        private readonly IStringLocalizer _t;

        public Handler(
            IRepositoryWithEvents<Domain.Catalog.Brand> repository,
            ICloudinaryFileStorageService cloudinaryFileStorageService,
            IStringLocalizer<Handler> t)
            => (_repository, _cloudinaryFileStorageService, _t) = (repository, cloudinaryFileStorageService, t);

        public async Task<string> Handle(Request request, CancellationToken cancellationToken)
        {
            var brand = request.Adapt(new Domain.Catalog.Brand());

            var logoUploadResult = request.Logo is not null
                ? await _cloudinaryFileStorageService.UploadAsync(request.Logo, FileType.Image, cancellationToken: cancellationToken)
                : null;
            brand.LogoUrl = logoUploadResult?.Url;

            await _repository.AddAsync(brand, cancellationToken);
            return _t["Brand created successfully."];
        }
    }

    public class EventHandler : EventNotificationHandler<EntityCreatedEvent<Domain.Catalog.Brand>>
    {
        private readonly ILogger<EventHandler> _logger;

        public EventHandler(ILogger<EventHandler> logger) => _logger = logger;

        public override Task Handle(EntityCreatedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
            return Task.CompletedTask;
        }
    }
}