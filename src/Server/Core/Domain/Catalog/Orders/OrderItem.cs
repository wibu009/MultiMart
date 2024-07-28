using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Catalog.Returns;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Orders;

public class OrderItem : AuditableEntity, IAggregateRoot
{
    public string? ProductName { get; set; }
    public decimal? BasePrice { get; set; }
    public decimal? Discount { get; set; }
    public int Quantity { get; set; }
    public DefaultIdType? OrderId { get; set; }
    public Orders.Order Order { get; set; } = default!;
    public DefaultIdType? ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public DefaultIdType? ReturnItemId { get; set; }
    public ReturnItem ReturnItem { get; set; } = default!;
}