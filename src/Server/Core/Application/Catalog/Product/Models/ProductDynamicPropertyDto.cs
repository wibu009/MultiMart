namespace MultiMart.Application.Catalog.Product.Models;

public class ProductDynamicPropertyDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public ProductDynamicPropertyValueDto<object> Value { get; set; } = new();
}