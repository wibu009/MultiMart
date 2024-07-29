using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class Category : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DefaultIdType? ParentId { get; set; }
    public Category Parent { get; set; } = default!;
    public List<Category> Children { get; set; } = new();
    public List<Product> Products { get; set; } = new();
}