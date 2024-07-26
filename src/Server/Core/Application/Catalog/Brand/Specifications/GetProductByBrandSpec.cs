using MultiMart.Domain.Catalog.Products;

namespace MultiMart.Application.Catalog.Brand.Specifications;

public sealed class GetProductByBrandSpec : Specification<Product>
{
    public GetProductByBrandSpec(DefaultIdType brandId)
    {
        Query.Where(x => x.BrandId == brandId);
    }
}