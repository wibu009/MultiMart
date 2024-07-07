using MultiMart.Application.Catalog.Product.Models;
using MultiMart.Application.Catalog.Product.Specifications;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Product.Requests.Queries;

public class SearchProductsRequest : PaginationFilter, IRequest<PaginationResponse<ProductDto>>
{
    public Guid? BrandId { get; set; }
    public decimal? MinimumRate { get; set; }
    public decimal? MaximumRate { get; set; }
}

public class SearchProductsRequestHandler : IRequestHandler<SearchProductsRequest, PaginationResponse<ProductDto>>
{
    private readonly IReadRepository<Domain.Catalog.Product> _repository;

    public SearchProductsRequestHandler(IReadRepository<Domain.Catalog.Product> repository) => _repository = repository;

    public async Task<PaginationResponse<ProductDto>> Handle(SearchProductsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ProductsBySearchRequestWithBrandsSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}