namespace MultiMart.Application.Catalog.Product.Specifications;

public class ProductByNameSpec : Specification<Domain.Catalog.Product>, ISingleResultSpecification
{
    public ProductByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}