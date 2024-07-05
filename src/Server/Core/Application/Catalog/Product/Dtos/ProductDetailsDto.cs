using MultiMart.Application.Catalog.Brand.Dtos;
using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Product.Dtos;

public class ProductDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public string? ImagePath { get; set; }
    public BrandDto Brand { get; set; } = default!;
}