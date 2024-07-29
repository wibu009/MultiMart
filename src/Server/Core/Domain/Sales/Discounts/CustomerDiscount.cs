using MultiMart.Domain.Sales.Orders;

namespace MultiMart.Domain.Sales.Discounts;

public class CustomerDiscount : Discount
{
    public string? CustomerId { get; set; }
    public List<OrderDiscount> Order { get; set; } = new();
}