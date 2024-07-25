﻿using MultiMart.Application.Common.FileStorage;

namespace MultiMart.Application.Catalog.Brand.Requests;

public class CreateBrandRequest : IRequest<string>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public FileUpload? Logo { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}