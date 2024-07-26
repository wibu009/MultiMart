using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class Brand : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public List<Product> Products { get; set; } = new();
}