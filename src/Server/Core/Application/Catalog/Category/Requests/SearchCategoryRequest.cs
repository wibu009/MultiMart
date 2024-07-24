using MultiMart.Application.Catalog.Category.Models;
using MultiMart.Application.Common.Models;

namespace MultiMart.Application.Catalog.Category.Requests;

public class SearchCategoryRequest : PaginationFilter, IRequest<PaginationResponse<CategoryDto>>
{
}