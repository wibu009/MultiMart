namespace MultiMart.Application.Catalog.V1.Category.Search;

public class SearchCategoryRequestHandler : IRequestHandler<SearchCategoryRequest, PaginationResponse<SearchCategoryResponseItem>>
{
    private readonly IReadRepository<Domain.Catalog.Category> _repository;
    private readonly IStringLocalizer _t;

    public SearchCategoryRequestHandler(
        IReadRepository<Domain.Catalog.Category> repository,
        IStringLocalizer<SearchCategoryRequestHandler> t)
    {
        _repository = repository;
        _t = t;
    }

    public async Task<PaginationResponse<SearchCategoryResponseItem>> Handle(
        SearchCategoryRequest request,
        CancellationToken cancellationToken)
        => await _repository.PaginatedListAsync(
            LambdaPaginationSpecification<Domain.Catalog.Category, SearchCategoryResponseItem>.Create(
                request,
                spec => spec.Query.OrderBy(c => c.CreatedOn, !request.HasOrderBy())),
            request.PageNumber,
            request.PageSize,
            cancellationToken);
}