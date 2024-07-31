namespace MultiMart.Application.Catalog.V1.Brand.Search;

public class SearchBrandResponse : PaginationResponse<SearchBrandResponseItem>
{
    public SearchBrandResponse(List<SearchBrandResponseItem> data, int count, int page, int pageSize)
        : base(data, count, page, pageSize)
    {
    }
}