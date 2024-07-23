using MultiMart.Application.Catalog.Brand.Models;

namespace MultiMart.Application.Catalog.Brand.Specifications;

public sealed class GetBrandByNameSpec : Specification<Domain.Catalog.Brand, BrandDto>, ISingleResultSpecification
{
    public GetBrandByNameSpec(string name)
        => Query.Where(b => b.Name == name);

    public GetBrandByNameSpec(string name, DefaultIdType id)
        => Query.Where(b => b.Name == name && b.Id != id);
}