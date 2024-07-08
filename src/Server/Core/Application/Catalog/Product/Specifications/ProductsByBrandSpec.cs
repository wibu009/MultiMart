namespace MultiMart.Application.Catalog.Product.Specifications;

public class ProductsByBrandSpec : Specification<Domain.Catalog.Product>
{
    public ProductsByBrandSpec(Guid brandId) =>
        Query.Where(p => p.BrandId == brandId);
}
