using MultiMart.Application.Catalog.Category.Models;
using MultiMart.Application.Catalog.Category.Requests;
using MultiMart.Application.Catalog.Category.Specifications;
using MultiMart.Application.Common.Models;

namespace MultiMart.Application.Catalog.Category.RequestHandlers;

public class SearchCategoryRequestHandler : IRequestHandler<SearchCategoryRequest, PaginationResponse<CategoryDto>>
{
    private readonly IReadRepository<Domain.Catalog.Category> _repository;

    public SearchCategoryRequestHandler(IReadRepository<Domain.Catalog.Category> repository)
        => _repository = repository;

    public async Task<PaginationResponse<CategoryDto>> Handle(SearchCategoryRequest request, CancellationToken cancellationToken)
        => await _repository.PaginatedListAsync(new SearchCategorySpec(request), request.PageNumber, request.PageSize, cancellationToken);
}