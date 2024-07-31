using MultiMart.Application.Catalog.Category.Models;

namespace MultiMart.Application.Catalog.Category.Requests;

public class GetCategoryByIdRequest : IRequest<CategoryDto>
{
    public DefaultIdType Id { get; set; }
    public GetCategoryByIdRequest(DefaultIdType id) => Id = id;
}