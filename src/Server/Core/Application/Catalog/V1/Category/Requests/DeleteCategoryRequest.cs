namespace MultiMart.Application.Catalog.Category.Requests;

public class DeleteCategoryRequest : IRequest<string>
{
    public DefaultIdType Id { get; set; }
    public DeleteCategoryRequest(DefaultIdType id) => Id = id;
}