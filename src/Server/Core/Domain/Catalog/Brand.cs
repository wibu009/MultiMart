﻿using MultiMart.Domain.Catalog.Products;
using MultiMart.Domain.Common.Contracts;

namespace MultiMart.Domain.Catalog;

public class Brand : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public IEnumerable<Product> Products { get; set; }
}