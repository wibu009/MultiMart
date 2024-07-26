using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Domain.Catalog.Discounts;

public class ProductDiscount : AuditableEntity, IAggregateRoot
{
    public int Quantity { get; set; }
    public string? Code { get; set; }
    public decimal Value { get; set; }
    public DiscountType Type { get; set; }
    public string? ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public string? DiscountId { get; set; }
    public Discount Discount { get; set; } = default!;
}