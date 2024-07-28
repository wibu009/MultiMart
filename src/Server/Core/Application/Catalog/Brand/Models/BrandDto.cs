﻿using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Catalog.Brand.Models;

public class BrandDto : IDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
}