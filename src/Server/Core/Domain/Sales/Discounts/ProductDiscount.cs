using MultiMart.Domain.Catalog.Products;

namespace MultiMart.Domain.Sales.Discounts;

public class ProductDiscount : Discount
{
    public DefaultIdType? ProductId { get; set; }
    public Product Product { get; set; } = default!;
}