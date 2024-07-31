namespace MultiMart.Application.Catalog.V1.Category.Delete;

public class DeleteCategoryRequest : IRequest<DeleteCategoryResponse>
{
    public DefaultIdType Id { get; set; }
    public DeleteCategoryRequest(DefaultIdType id) => Id = id;
}