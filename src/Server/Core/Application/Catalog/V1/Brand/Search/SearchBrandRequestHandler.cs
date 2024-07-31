namespace MultiMart.Application.Catalog.V1.Brand.Search;

public class SearchBrandRequestHandler : IRequestHandler<SearchBrandRequest, PaginationResponse<SearchBrandResponseItem>>
{
    private readonly IReadRepository<Domain.Catalog.Brand> _repository;
    private readonly IStringLocalizer _t;

    public SearchBrandRequestHandler(
        IReadRepository<Domain.Catalog.Brand> repository,
        IStringLocalizer<SearchBrandRequestHandler> t)
    {
        _repository = repository;
        _t = t;
    }

    public async Task<PaginationResponse<SearchBrandResponseItem>> Handle(SearchBrandRequest request, CancellationToken cancellationToken)
        => await _repository.PaginatedListAsync(
            LambdaPaginationSpecification<Domain.Catalog.Brand, SearchBrandResponseItem>
            .Create(request, spec => spec.Query.OrderBy(c => c.CreatedOn, !request.HasOrderBy())),
            request.PageNumber,
            request.PageSize,
            cancellationToken);
}