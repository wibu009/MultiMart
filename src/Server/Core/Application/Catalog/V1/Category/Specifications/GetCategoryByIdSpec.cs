using MultiMart.Application.Catalog.Category.Models;

namespace MultiMart.Application.Catalog.Category.Specifications;

public sealed class GetCategoryByIdSpec : Specification<Domain.Catalog.Category, CategoryDto>, ISingleResultSpecification
{
    public GetCategoryByIdSpec(DefaultIdType id)
        => Query
            .Include(c => c.Children)
            .Where(c => c.Id == id);
}