using MultiMart.Domain.Catalog.Orders;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Returns;

public class ReturnItem : AuditableEntity, IAggregateRoot
{
    public int Quantity { get; set; }
    public string? Note { get; set; }
    public string? ReturnId { get; set; }
    public Return Return { get; set; } = default!;
    public string? OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; } = default!;
}