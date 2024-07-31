using System.Text.Json.Serialization;
using MultiMart.Application.Common.FileStorage;

namespace MultiMart.Application.Catalog.V1.Brand.Update;

public class UpdateBrandRequest : IRequest<UpdateBrandResponse>
{
    [JsonIgnore]
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public FileUpload? Logo { get; set; }
    public bool DeleteCurrentLogo { get; set; }
}