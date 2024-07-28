using MultiMart.Domain.Catalog.Discounts;
using MultiMart.Domain.Catalog.Orders;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Products;

public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public string? MeasurementUnit { get; set; }
    public string? SKU { get; set; }
    public decimal? Weight { get; set; }
    public string? Dimensions { get; set; }
    public bool IsPublished { get; set; }
    public int? MinimumOrderQuantity { get; set; }
    public int? MaximumOrderQuantity { get; set; }
    public bool IsAllowToOrderWhenOutOfStock { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public DefaultIdType? BrandId { get; set; }
    public Brand Brand { get; set; } = default!;
    public DefaultIdType? CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public DefaultIdType? SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;
    public List<OrderItem> OrderItems { get; set; } = new();
    public List<ProductDiscount> Discounts { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}