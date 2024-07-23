using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class Category : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int? ParentId { get; set; }
    public Category? Parent { get; set; }
    public List<Category> Children { get; set; } = new();
    public List<Product.Product> Products { get; set; } = new();
}