using MultiMart.Domain.Catalog.Orders;

namespace MultiMart.Domain.Catalog.Discounts;

public class CustomerDiscount : Discount
{
    public string? CustomerId { get; set; }
    public List<OrderDiscount> Order { get; set; } = new();
}