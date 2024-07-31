namespace MultiMart.Application.Catalog.V1.Category.Get;

public class GetCategoryRequest : IRequest<GetCategoryResponse>
{
    public DefaultIdType Id { get; set; }
    public GetCategoryRequest(DefaultIdType id) => Id = id;
}