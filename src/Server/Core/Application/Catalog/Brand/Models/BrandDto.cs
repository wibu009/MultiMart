using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Brand.Models;

public class BrandDto : IDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}