namespace MultiMart.Application.Catalog.V1.Category.Create;

public class CreateCategoryRequest : IRequest<CreateCategoryResponse>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DefaultIdType? ParentId { get; set; }
}