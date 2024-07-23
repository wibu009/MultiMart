using MultiMart.Application.Catalog.Brand.Models;

namespace MultiMart.Application.Catalog.Brand.Specifications;

public sealed class GetBrandByIdSpec : Specification<Domain.Catalog.Brand, BrandDto>, ISingleResultSpecification
{
    public GetBrandByIdSpec(DefaultIdType id)
        => Query.Where(b => b.Id == id);
}