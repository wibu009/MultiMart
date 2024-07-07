using MultiMart.Application.Catalog.Brand.Models;
using MultiMart.Application.Catalog.Brand.Requests.Queries;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Specification;

namespace MultiMart.Application.Catalog.Brand.Specifications;

public class BrandsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Domain.Catalog.Brand, BrandDto>
{
    public BrandsBySearchRequestSpec(SearchBrandsRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}