using MultiMart.Domain.Catalog.Order;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Product;

public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public string? SKU { get; set; }
    public bool IsPublished { get; set; }
    public int? MinimumOrderQuantity { get; set; }
    public int? MaximumOrderQuantity { get; set; }
    public bool IsAllowToOrderWhenOutOfStock { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public DefaultIdType? BrandId { get; set; }
    public Brand Brand { get; set; } = default!;
    public DefaultIdType? CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public List<OrderItem> OrderItems { get; set; } = new();
}