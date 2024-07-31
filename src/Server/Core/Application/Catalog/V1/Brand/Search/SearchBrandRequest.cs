namespace MultiMart.Application.Catalog.V1.Brand.Search;

public class SearchBrandRequest : PaginationFilter, IRequest<PaginationResponse<SearchBrandResponseItem>>
{
}