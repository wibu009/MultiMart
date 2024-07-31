using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.V1.Brand.Get;

public class GetBrandResponse : IDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
}