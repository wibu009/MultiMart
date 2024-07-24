using MultiMart.Domain.Catalog.Product;

namespace MultiMart.Application.Catalog.Category.Specifications;

public sealed class GetProductByCategorySpec : Specification<Product>
{
    public GetProductByCategorySpec(DefaultIdType categoryId)
        => Query.Where(x => x.CategoryId == categoryId);
}