using MultiMart.Application.Common.Exceptions;
using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Brand.V1;

public abstract class GetBrandByIdQuery
{
    public class Request : IRequest<Response>
    {
        public DefaultIdType Id { get; set; }
    }

    public abstract class Response : IDto
    {
        public DefaultIdType Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }

    private sealed class GetBrandByIdSpec : Specification<Domain.Catalog.Brand, Response>, ISingleResultSpecification
    {
        public GetBrandByIdSpec(DefaultIdType id)
            => Query.Where(b => b.Id == id);
    }

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IReadRepository<Domain.Catalog.Brand> _repository;
        private readonly IStringLocalizer _t;

        public Handler(IReadRepository<Domain.Catalog.Brand> repository, IStringLocalizer<Handler> t)
            => (_repository, _t) = (repository, t);

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            => await _repository.FirstOrDefaultAsync(new GetBrandByIdSpec(request.Id), cancellationToken)
               ?? throw new NotFoundException(_t["Brand not found"]);
    }
}