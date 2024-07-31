using MultiMart.Application.Common.FileStorage;

namespace MultiMart.Application.Catalog.V1.Brand.Create;

public class CreateBrandRequest : IRequest<CreateBrandResponse>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public FileUpload? Logo { get; set; }
}