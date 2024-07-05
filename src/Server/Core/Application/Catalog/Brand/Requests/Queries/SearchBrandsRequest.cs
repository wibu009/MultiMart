using MultiMart.Application.Catalog.Brand.Dtos;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Brand.Requests.Queries;

public class SearchBrandsRequest : PaginationFilter, IRequest<PaginationResponse<BrandDto>>
{
}

public class SearchBrandsRequestHandler : IRequestHandler<SearchBrandsRequest, PaginationResponse<BrandDto>>
{
    private readonly IReadRepository<Domain.Catalog.Brand> _repository;

    public SearchBrandsRequestHandler(IReadRepository<Domain.Catalog.Brand> repository) => _repository = repository;

    public async Task<PaginationResponse<BrandDto>> Handle(SearchBrandsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BrandsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}