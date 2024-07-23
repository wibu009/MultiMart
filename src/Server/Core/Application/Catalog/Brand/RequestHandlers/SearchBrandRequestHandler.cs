using MultiMart.Application.Catalog.Brand.Models;
using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Catalog.Brand.Specifications;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Brand.RequestHandlers;

public class SearchBrandRequestHandler : IRequestHandler<SearchBrandRequest, PaginationResponse<BrandDto>>
{
    private readonly IReadRepository<Domain.Catalog.Brand> _repository;

    public SearchBrandRequestHandler(IReadRepository<Domain.Catalog.Brand> repository)
        => _repository = repository;

    public async Task<PaginationResponse<BrandDto>> Handle(SearchBrandRequest request, CancellationToken cancellationToken)
        => await _repository.PaginatedListAsync(new SearchBrandSpec(request), request.PageNumber, request.PageSize, cancellationToken);
}