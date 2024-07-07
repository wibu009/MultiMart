using MultiMart.Application.Catalog.Product.Models;

namespace MultiMart.Application.Catalog.Product.Specifications;

public class ProductByIdWithBrandSpec : Specification<Domain.Catalog.Product, ProductDetailsDto>, ISingleResultSpecification
{
    public ProductByIdWithBrandSpec(Guid id) =>
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Brand);
}