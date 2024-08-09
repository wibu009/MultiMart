using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Sales.Returns;

namespace MultiMart.Domain.Sales.Orders;

public class OrderItem : AuditableEntity, IAggregateRoot
{
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public decimal? Discount { get; set; }
    public int Quantity { get; set; }
    public DefaultIdType? OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public DefaultIdType? ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public DefaultIdType? ReturnItemId { get; set; }
    public ReturnItem ReturnItem { get; set; } = default!;
}