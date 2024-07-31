namespace MultiMart.Application.Catalog.V1.Category.Search;

public class SearchCategoryResponse : PaginationResponse<SearchCategoryResponseItem>
{
    public SearchCategoryResponse(List<SearchCategoryResponseItem> data, int count, int page, int pageSize)
        : base(data, count, page, pageSize)
    {
    }
}