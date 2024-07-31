using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.V1.Category.Search;

public class SearchCategoryResponseItem : IDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DefaultIdType? ParentId { get; set; }
}