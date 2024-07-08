namespace MultiMart.Application.Catalog.Brand.Specifications;

public class BrandByNameSpec : Specification<Domain.Catalog.Brand>, ISingleResultSpecification
{
    public BrandByNameSpec(string name) =>
        Query.Where(b => b.Name == name);
}