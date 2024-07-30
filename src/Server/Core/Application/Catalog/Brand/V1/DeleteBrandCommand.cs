using MultiMart.Application.Common.Events;
using MultiMart.Application.Common.Validation;
using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Common.Events;

namespace MultiMart.Application.Catalog.Brand.V1;

public abstract class DeleteBrandCommand
{
    public class Request : IRequest<string>
    {
        public DefaultIdType Id { get; init; }
    }

    private sealed class GetBrandByIdSpec : Specification<Domain.Catalog.Brand>, ISingleResultSpecification
    {
        public GetBrandByIdSpec(DefaultIdType id)
            => Query.Where(b => b.Id == id);
    }

    private sealed class GetProductByBrandSpec : Specification<Product>
    {
        public GetProductByBrandSpec(DefaultIdType brandId)
            => Query.Where(x => x.BrandId == brandId);
    }

    public class Validator : CustomValidator<Request>
    {
        public Validator(
            IReadRepository<Domain.Catalog.Brand> brandRepository,
            IReadRepository<Product> productRepository,
            IStringLocalizer<Validator> t)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(t["Id is required."])
                .MustAsync(async (id, cancellationToken)
                    => !await brandRepository.AnyAsync(new GetBrandByIdSpec(id), cancellationToken))
                .WithMessage(t["Brand not found."])
                .MustAsync(async (id, cancellationToken)
                    => await productRepository.AnyAsync(new GetProductByBrandSpec(id), cancellationToken))
                .WithMessage(t["Brand has products, cannot delete."]);
        }
    }

    public class Handler : IRequestHandler<Request, string>
    {
        private readonly IRepositoryWithEvents<Domain.Catalog.Brand> _repository;
        private readonly IStringLocalizer _t;

        public Handler(
            IRepositoryWithEvents<Domain.Catalog.Brand> repository,
            IStringLocalizer<Handler> t)
            => (_repository, _t) = (repository, t);

        public async Task<string> Handle(Request request, CancellationToken cancellationToken)
        {
            var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);
            await _repository.DeleteAsync(brand!, cancellationToken);
            return _t["Brand deleted successfully."];
        }
    }

    public class EventHandler : EventNotificationHandler<EntityDeletedEvent<Domain.Catalog.Brand>>
    {
        private readonly ILogger<EventHandler> _logger;

        public EventHandler(ILogger<EventHandler> logger) => _logger = logger;

        public override Task Handle(EntityDeletedEvent<Domain.Catalog.Brand> @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Event} Triggered", @event.GetType().Name);
            return Task.CompletedTask;
        }
    }
}