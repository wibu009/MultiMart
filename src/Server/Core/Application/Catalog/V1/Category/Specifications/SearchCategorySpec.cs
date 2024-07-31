using MultiMart.Application.Catalog.Category.Models;
using MultiMart.Application.Catalog.Category.Requests;

namespace MultiMart.Application.Catalog.Category.Specifications;

public sealed class SearchCategorySpec : EntitiesByPaginationFilterSpec<Domain.Catalog.Category, CategoryDto>
{
    public SearchCategorySpec(SearchCategoryRequest request)
        : base(request)
        => Query
            .OrderBy(c => c.CreatedOn, !request.HasOrderBy());
}