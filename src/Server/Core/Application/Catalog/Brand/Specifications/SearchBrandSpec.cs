using MultiMart.Application.Catalog.Brand.Models;
using MultiMart.Application.Catalog.Brand.Requests;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Specification;

namespace MultiMart.Application.Catalog.Brand.Specifications;

public sealed class SearchBrandSpec : EntitiesByPaginationFilterSpec<Domain.Catalog.Brand, BrandDto>
{
    public SearchBrandSpec(SearchBrandRequest request)
        : base(request)
        => Query.OrderBy(c => c.CreatedOn, !request.HasOrderBy());
}