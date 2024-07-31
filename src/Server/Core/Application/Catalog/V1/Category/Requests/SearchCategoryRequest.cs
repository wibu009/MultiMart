using MultiMart.Application.Catalog.Category.Models;

namespace MultiMart.Application.Catalog.Category.Requests;

public class SearchCategoryRequest : PaginationFilter, IRequest<PaginationResponse<CategoryDto>>
{
}