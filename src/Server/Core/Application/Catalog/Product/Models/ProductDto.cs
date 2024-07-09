using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Product.Models;

public class ProductDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public string? ImagePath { get; set; }
    public DefaultIdType BrandId { get; set; }
    public string BrandName { get; set; } = default!;
    public List<ProductDynamicPropertyDto> DynamicProperties { get; set; } = new();
}