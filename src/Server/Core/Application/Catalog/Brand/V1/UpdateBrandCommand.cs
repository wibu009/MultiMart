using System.Text.Json.Serialization;
using MultiMart.Application.Common.Events;
using MultiMart.Application.Common.FileStorage;
using MultiMart.Application.Common.FileStorage.Cloudinary;
using MultiMart.Application.Common.Validation;
using MultiMart.Domain.Common.Enums;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Brand.V1;

public abstract class UpdateBrandCommand
{
    public class Request : IRequest<string>
    {
        [JsonIgnore]
        public DefaultIdType Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public FileUpload? Logo { get; set; }
        public bool DeleteCurrentLogo { get; set; }
    }

    private sealed class GetBrandByIdSpec : Specification<Domain.Catalog.Brand>, ISingleResultSpecification
    {
        public GetBrandByIdSpec(DefaultIdType id)
            => Query.Where(b => b.Id == id);
    }

    private sealed class GetBrandByNameSpec : Specification<Domain.Catalog.Brand>, ISingleResultSpecification
    {
        public GetBrandByNameSpec(string name, DefaultIdType id)
            => Query.Where(b => b.Name == name && b.Id != id);
    }

    public class Validator : CustomValidator<Request>
    {
        public Validator(IReadRepository<Domain.Catalog.Brand> repository, IStringLocalizer<Validator> t)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(t["Id is required"])
                .MustAsync(async (id, cancellationToken) => await repository.AnyAsync(new GetBrandByIdSpec(id), cancellationToken))
                .WithMessage(t["Brand not found"]);

            RuleFor(x => x.Name)
                .MaximumLength(255)
                .WithMessage(t["Name is too long"])
                .MustAsync(async (request, name, token) => await repository.FirstOrDefaultAsync(new GetBrandByNameSpec(name, request.Id), token) is null)
                .WithMessage((_, name) => t["Brand with {0} already exists.", name]);

            RuleFor(x => x.Description)
                .MaximumLength(4000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage(t["Description is too long"]);
        }
    }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Request, Domain.Catalog.Brand>()
                .Ignore(dest => dest.Id)
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
            var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);
            brand = request.Adapt(brand);

            if (request.DeleteCurrentLogo)
            {
                if (!string.IsNullOrWhiteSpace(brand.LogoUrl))
                {
                    await _cloudinaryFileStorageService.RemoveAsync(brand.LogoUrl, cancellationToken: cancellationToken);
                    brand.LogoUrl = null;
                }
            }
            else if (request.Logo != null)
            {
                if (!string.IsNullOrWhiteSpace(brand.LogoUrl))
                {
                    await _cloudinaryFileStorageService.RemoveAsync(brand.LogoUrl, cancellationToken: cancellationToken);
                }

                var uploadResult = await _cloudinaryFileStorageService.UploadAsync(request.Logo, FileType.Image, cancellationToken: cancellationToken);
                brand.LogoUrl = uploadResult.Url;
            }

            await _repository.UpdateAsync(brand!, cancellationToken);

            return _t["Brand updated successfully."];
        }
    }

    public class EventHandler : EventNotificationHandler<EntityUpdatedEvent<Domain.Catalog.Brand>>
    {
        private readonly ILogger<EventHandler> _logger;

        public EventHandler(ILogger<EventHandler> logger) => _logger = logger;

        public override Task Handle(EntityUpdatedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
            return Task.CompletedTask;
        }
    }
}