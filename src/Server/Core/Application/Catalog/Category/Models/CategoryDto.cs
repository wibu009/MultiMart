using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Category.Models;

public class CategoryDto : IDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DefaultIdType? ParentId { get; set; }
}