namespace MultiMart.Application.Catalog.V1.Category.Update;

public class UpdateCategoryRequest : IRequest<UpdateCategoryResponse>
{
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DefaultIdType? ParentId { get; set; }
}