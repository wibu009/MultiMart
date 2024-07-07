using MultiMart.Application.Catalog.Brand.Models;
using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Product.Models;

public class ProductDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public string? ImagePath { get; set; }
    public BrandDto Brand { get; set; } = default!;
}