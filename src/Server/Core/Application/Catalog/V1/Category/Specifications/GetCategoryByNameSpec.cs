using MultiMart.Application.Catalog.Category.Models;

namespace MultiMart.Application.Catalog.Category.Specifications;

public sealed class GetCategoryByNameSpec : Specification<Domain.Catalog.Category, CategoryDto>
{
    public GetCategoryByNameSpec(string name)
        => Query
            .Where(c => c.Name == name);

    public GetCategoryByNameSpec(string name, DefaultIdType id)
        => Query
            .Where(c => c.Name == name && c.Id != id);
}