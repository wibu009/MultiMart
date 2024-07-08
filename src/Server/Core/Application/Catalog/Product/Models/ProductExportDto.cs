using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Product.Models;

public class ProductExportDto : IDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Rate { get; set; } = default!;
    public string BrandName { get; set; } = default!;
}