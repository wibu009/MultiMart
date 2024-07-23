using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Order;

public class OrderItem : AuditableEntity, IAggregateRoot
{
    public string? OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public string? ProductId { get; set; }
    public Product.Product Product { get; set; } = default!;
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public int Quantity { get; set; }
}