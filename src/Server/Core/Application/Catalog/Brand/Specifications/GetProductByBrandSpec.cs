using MultiMart.Domain.Catalog.Product;

namespace MultiMart.Application.Catalog.Brand.Specifications;

public sealed class GetProductByBrandSpec : Specification<Product>
{
    public GetProductByBrandSpec(DefaultIdType brandId)
    {
        Query.Where(x => x.BrandId == brandId);
    }
}