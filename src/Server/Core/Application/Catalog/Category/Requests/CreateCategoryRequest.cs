namespace MultiMart.Application.Catalog.Category.Requests;

public class CreateCategoryRequest : IRequest<string>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DefaultIdType? ParentId { get; set; }
}