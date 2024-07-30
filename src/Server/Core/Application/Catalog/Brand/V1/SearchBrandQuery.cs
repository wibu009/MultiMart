using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Specification;

namespace MultiMart.Application.Catalog.Brand.V1;

public abstract class SearchBrandQuery
{
    public class Request : PaginationFilter, IRequest<PaginationResponse<ResponseItem>>
    {
    }

    public abstract class ResponseItem : IDto
    {
        public DefaultIdType Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }

    private sealed class SearchBrandSpec : EntitiesByPaginationFilterSpec<Domain.Catalog.Brand, ResponseItem>
    {
        public SearchBrandSpec(Request request)
            : base(request)
            => Query.OrderBy(c => c.CreatedOn, !request.HasOrderBy());
    }

    public class Handler : IRequestHandler<Request, PaginationResponse<ResponseItem>>
    {
        private readonly IReadRepository<Domain.Catalog.Brand> _repository;

        public Handler(IReadRepository<Domain.Catalog.Brand> repository)
            => _repository = repository;

        public async Task<PaginationResponse<ResponseItem>> Handle(Request request, CancellationToken cancellationToken)
            => await _repository.PaginatedListAsync(new SearchBrandSpec(request), request.PageNumber, request.PageSize, cancellationToken);
    }
}