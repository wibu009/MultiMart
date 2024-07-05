using MultiMart.Application.Catalog.Brand.Dtos;

namespace MultiMart.Application.Catalog.Brand.Specifications;

public class BrandByIdSpec : Specification<Domain.Catalog.Brand, BrandDto>, ISingleResultSpecification
{
    public BrandByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}