using MultiMart.Application.Catalog.Category.Models;
using MultiMart.Application.Catalog.Category.Requests;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Specification;

namespace MultiMart.Application.Catalog.Category.Specifications;

public sealed class SearchCategorySpec : EntitiesByPaginationFilterSpec<Domain.Catalog.Category, CategoryDto>
{
    public SearchCategorySpec(SearchCategoryRequest request)
        : base(request)
        => Query
            .OrderBy(c => c.CreatedOn, !request.HasOrderBy());
}