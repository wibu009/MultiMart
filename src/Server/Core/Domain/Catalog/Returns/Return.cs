using MultiMart.Domain.Catalog.Orders;
using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Domain.Catalog.Returns;

public class Return : AuditableEntity, IAggregateRoot
{
    public string? Reason { get; set; }
    public ReturnType Type { get; set; }
    public ReturnStatus Status { get; set; }
    public decimal? RefundAmount { get; set; }
    public string? Note { get; set; }
    public string? OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public List<ReturnItem> ReturnItems { get; set; } = new();
}