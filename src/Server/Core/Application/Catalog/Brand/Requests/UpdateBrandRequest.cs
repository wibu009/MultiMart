﻿using System.Text.Json.Serialization;
using MultiMart.Application.Common.FileStorage;

namespace MultiMart.Application.Catalog.Brand.Requests;

public class UpdateBrandRequest : IRequest<string>
{
    [JsonIgnore]
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public FileUpload? Logo { get; set; }
    public bool DeleteCurrentLogo { get; set; }
}