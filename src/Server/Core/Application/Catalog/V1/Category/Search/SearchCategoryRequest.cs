namespace MultiMart.Application.Catalog.V1.Category.Search;

public class SearchCategoryRequest : PaginationFilter, IRequest<PaginationResponse<SearchCategoryResponseItem>>
{
}