using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog.Return;

public class ReturnItem : AuditableEntity, IAggregateRoot
{
    public int Quantity { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public string? ReturnId { get; set; }
    public Return Return { get; set; } = default!;
    public string? OrderItemId { get; set; }
    public Order.OrderItem OrderItem { get; set; } = default!;
}