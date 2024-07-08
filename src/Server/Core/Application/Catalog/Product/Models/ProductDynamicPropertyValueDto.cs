namespace MultiMart.Application.Catalog.Product.Models;

public class ProductDynamicPropertyValueDto<T>
{
    public DefaultIdType Id { get; set; }
    public T Value { get; set; } = default!;
}