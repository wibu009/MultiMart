namespace MultiMart.Application.Catalog.Category.Requests;

public class UpdateCategoryRequest : IRequest<string>
{
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DefaultIdType? ParentId { get; set; }
}