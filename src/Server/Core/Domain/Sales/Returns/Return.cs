using MultiMart.Domain.Common.Contracts;
using MultiMart.Domain.Common.Enums;
using MultiMart.Domain.Sales.Orders;

namespace MultiMart.Domain.Sales.Returns;

public class Return : AuditableEntity, IAggregateRoot
{
    public string? Reason { get; set; }
    public ReturnType Type { get; set; }
    public ReturnStatus Status { get; set; }
    public decimal? TotalRefund { get; set; }
    public string? Note { get; set; }
    public string? OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public List<ReturnItem> Items { get; set; } = new();
}