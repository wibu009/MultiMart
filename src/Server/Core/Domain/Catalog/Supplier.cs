using MultiMart.Domain.Catalog.Addresses;
using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class Supplier : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public List<AddressOfSupplier> Addresses { get; set; } = new();
    public List<Product> Products { get; set; } = new();
}